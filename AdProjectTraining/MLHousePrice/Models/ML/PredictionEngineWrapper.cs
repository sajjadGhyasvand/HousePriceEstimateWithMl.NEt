using Microsoft.ML;
namespace MLHousePrice.Models.ML
{
    public class PredictionEngineWrapper
    {
        private readonly PredictionEngine<MLInputData, OutPutData> _predictionEngine;

        public PredictionEngineWrapper(ITransformer trainedModel , MLContext mlcontext)
        {
            try
            {
            _predictionEngine = mlcontext.Model.CreatePredictionEngine<MLInputData, OutPutData>(trainedModel);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public OutPutData Predict(MLInputData inputData)
        {
            return _predictionEngine.Predict(inputData);
        }
    }
}
