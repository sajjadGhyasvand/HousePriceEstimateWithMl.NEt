
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
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
    .Append(context.Regression.Trainers.FastTree(new FastTreeRegressionTrainer.Options
    {
        NumberOfLeaves = 20,
        NumberOfTrees = 100,
        LearningRate = 0.1
    }))
    .Append(context.Transforms.CopyColumns("Score", "Score"));

var tts = context.Data.TrainTestSplit(data, testFraction: 0.2);

var trainedModel = trainingPipeline.Fit(tts.TrainSet);

// Save model
context.Model.Save(trainedModel, data.Schema, "model.zip");


var predEngine = context.Model.CreatePredictionEngine<Advertisment, HousePricePrediction>(trainedModel);

var result = predEngine.Predict(new Advertisment
{
Area = 157,
BuildYear = 1385,
Rooms = 3,
Floor = 2,
Elevator = true,
Parking = true,
Storage = true,
LocationName = "سعادت آباد"
});

Console.WriteLine($"Predicted price:{result.Price.ToString("n0")}");


//=================Model Assesment
var predictions = trainedModel.Transform(tts.TestSet);
var metrics = context.Regression.Evaluate(predictions);
Console.WriteLine($"R^2: {metrics.RSquared:0.##}");
Console.WriteLine($"Mean Absolute Error(MAE) :{metrics.MeanAbsoluteError.ToString("n0")} Toman");
Console.WriteLine($"Mean Squared Error(MSE) : {metrics.MeanSquaredError.ToString("n0")} Toman ");
Console.WriteLine($"Root Mean Squared Error(RMSE): {metrics.RootMeanSquaredError.ToString("n0")} Toman");


static List<Advertisment> FetchAdvertisements()
{
var advertisements = new List<Advertisment>();
string connectionString = "Server=.;Database=AdvertisementsDB;Integrated Security=True;";

using (SqlConnection connection = new SqlConnection(connectionString))
{
connection.Open();

string query = @"SELECT a.Area, a.BuildYear, a.Rooms, a.TotalPrice, a.Floor, a.Elevator, a.Parking, a.Storage, l.Name
                         FROM Advertisements a
                         INNER JOIN Locations l ON a.LocationId = l.Id";

using (SqlCommand command = new SqlCommand(query, connection))
using (SqlDataReader reader = command.ExecuteReader())
{
while (reader.Read())
{
var ad = new Advertisment
{
Area = (int)reader.GetInt32(0),
BuildYear = (int)reader.GetInt32(1),
Rooms = (int)reader.GetInt32(2),
TotalPrice = (long)reader.GetInt64(3),
Floor = (int)reader.GetInt32(4),
Elevator = reader.GetBoolean(5),
Parking = reader.GetBoolean(6),
Storage = reader.GetBoolean(7),
LocationName = reader.GetString(8)
};

advertisements.Add(ad);
}
}

connection.Close();
}

return advertisements;
}

public class HousePricePrediction
{
    [ColumnName("Score")]
    public float Price { get; set; }

}