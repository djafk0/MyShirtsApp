namespace MyShirtsApp.Test.Mocks
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;

    public class TempDataMock
    {
        public static ITempDataDictionary Instance
        {
            get
            {
                var tempDataProvider = Mock.Of<ITempDataProvider>();

                var tempDataDictrionaryFactory = new TempDataDictionaryFactory(tempDataProvider);

                var tempData = tempDataDictrionaryFactory.GetTempData(new DefaultHttpContext());

                return tempData;
            }
        }
    }
}
