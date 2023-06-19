using Microsoft.ML;
using Newtonsoft.Json.Linq;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var mlContext = new MLContext();

// ---------------------- Load Data

var dataPath = "output.csv";
var textLoader = mlContext.Data.CreateTextLoader<Advertisement>(separatorChar: ',', hasHeader:true);
 
var data = textLoader.Load(dataPath);

var replaceMissing = HandleMissingValues(mlContext, data);

var normalizedData =  NormalizeData(mlContext, replaceMissing);

IDataView encodedData=EncodeCategoricalValues(mlContext,normalizedData);

SaveCleanedData(mlContext, encodedData, "cleanedData.csv");

//------------------------------------------------------------
static void SaveCleanedData(MLContext mLContext,IDataView cleanedData , string outputCsvFile)
{
    cleanedData = mLContext.Transforms.SelectColumns(new[] { "Area", "LocationName" })
            .Fit(cleanedData).Transform(cleanedData);
    using (var fileStream = File.Create(outputCsvFile))
    {
        mLContext.Data.SaveAsText(cleanedData, fileStream, separatorChar: ',', headerRow: true, schema: true);
    }
}


static IDataView HandleMissingValues(MLContext mLContext, IDataView data)
{
    var PipeLine = mLContext.Transforms.ReplaceMissingValues("Area",
         replacementMode: Microsoft.ML.Transforms.MissingValueReplacingEstimator.ReplacementMode.Mean)
         .Append(mLContext.Transforms.ReplaceMissingValues(nameof(Advertisement.BuildYear),
         replacementMode: Microsoft.ML.Transforms.MissingValueReplacingEstimator.ReplacementMode.Mode))
          .Append(mLContext.Transforms.ReplaceMissingValues(nameof(Advertisement.Rooms),
         replacementMode: Microsoft.ML.Transforms.MissingValueReplacingEstimator.ReplacementMode.Mean))
           .Append(mLContext.Transforms.ReplaceMissingValues(nameof(Advertisement.Floor),
         replacementMode: Microsoft.ML.Transforms.MissingValueReplacingEstimator.ReplacementMode.Mean));

    var transformedData = PipeLine.Fit(data).Transform(data);

    var roundingPipeline = mLContext.Transforms.CustomMapping
        (new Action<Advertisement, AdvertisementRounded>(AdvertisementMapping.MapRounded)
        , contractName: null);
    var roundedData= roundingPipeline.Fit(transformedData).Transform(transformedData);

    var combinedPipeline = mLContext.Transforms.CopyColumns("Area", "AreaRounded")
        .Append(mLContext.Transforms.CopyColumns("Rooms", "RoomsRounded"))
        .Append(mLContext.Transforms.CopyColumns("Floor", "FloorRounded"));

    var tempData = combinedPipeline.Fit(roundedData).Transform(roundedData);

    var finalData = mLContext.Transforms.SelectColumns(new[] { "Area", "BuildYear", "Rooms", "Floor", "Elevator", "Parking", "Storage", "LocationName", "TotalPrice" })
        .Fit(tempData).Transform(tempData);
    return finalData;
}

 static IDataView NormalizeData(MLContext mlContext, IDataView data)
{
    var pipeline = mlContext.Transforms.NormalizeMinMax("Area", "Area")
        .Append(mlContext.Transforms.NormalizeMinMax("BuildYear", "BuildYear"))
        .Append(mlContext.Transforms.NormalizeMinMax("Rooms", "Rooms"))
        .Append(mlContext.Transforms.NormalizeMinMax("Floor", "Floor"));

    return pipeline.Fit(data).Transform(data);
}

static IDataView EncodeCategoricalValues(MLContext mlContext, IDataView data)
{
    var pipeline = mlContext.Transforms.Categorical.OneHotHashEncoding("LocationName", "LocationName");
    return pipeline.Fit(data).Transform(data);
}


public class AdvertisementRounded
{
    public float AreaRounded;
    public float RoomsRounded;
    public float FloorRounded;
}


public class AdvertisementMapping
{
    public static void MapRounded(Advertisement input, AdvertisementRounded output)
    {
        output.AreaRounded = (float)Math.Round(input.Area);
        output.RoomsRounded = (float)Math.Round(input.Rooms);
        output.FloorRounded = (float)Math.Round(input.Floor);
    }
}


