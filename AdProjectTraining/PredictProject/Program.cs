
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Data.SqlClient;

var context = new MLContext();
var allData = FetchAdvertisements();
// Load Data
var data = context.Data.LoadFromEnumerable(allData);

//PreProcess Data

var preprocessingPipeline = context.Transforms.Categorical
    .OneHotEncoding("LocationNameEncoded", "LocationName")
    .Append(context.Transforms.NormalizeMinMax("AreaNormalized", "Area"))
    .Append(context.Transforms.NormalizeMinMax("BuildYearNormalized", "BuildYear"))
    .Append(context.Transforms.NormalizeMinMax("RoomsNormalized", "Rooms"))
    .Append(context.Transforms.NormalizeMinMax("FloorNormalized", "Floor"))
    .Append(context.Transforms.Conversion
    .ConvertType("ElevatorConverted", "Elevator", outputKind: Microsoft.ML.Data.DataKind.Single))
     .Append(context.Transforms.Conversion
    .ConvertType("ParkingConverted", "Parking", outputKind: Microsoft.ML.Data.DataKind.Single))
     .Append(context.Transforms.Conversion
    .ConvertType("StorageConverted", "Storage", outputKind: Microsoft.ML.Data.DataKind.Single))
    .Append(context.Transforms.Concatenate("Features", "AreaNormalized", "BuildYearNormalized"
    , "RoomsNormalized", "FloorNormalized", "ElevatorConverted",
    "ParkingConverted", "StorageConverted", "LocationNameEncoded"));

//Train Model
var trainingPipeline = preprocessingPipeline
    .Append(context.Transforms.CopyColumns("Label", "TotalPrice"))
    .Append(context.Regression.Trainers.Sdca())
    .Append(context.Transforms.CopyColumns("Score", "Score"));

var tts = context.Data.TrainTestSplit(data, testFraction: 0.2);

var trainedModel = trainingPipeline.Fit(tts.TrainSet);

