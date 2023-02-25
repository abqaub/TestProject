using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;


namespace TestProject
{
    public class DemoTest
    {
        private UtilityClass uc; 
        public DemoTest()
        {
            uc = new UtilityClass();
        }
        
        [Fact]
        public void Test1()
        {
            var client = new RestClient("https://openlibrary.org");
            var request = new RestRequest("search.json", Method.Get);
            request.AddParameter("title", "Goodnight+Moon");

            var response = client.ExecuteGet(request);
            var deserializedResponse = JsonConvert.DeserializeObject<BookClass>(response.Content);

            // 3.1.1. Prints out the total number of books with the title matching exactly [Goodnight Moon]

            IList<Doc> listOfDoc = deserializedResponse.docs;
            IList<string> book = new List<string>();
            foreach (var titleGoodNightMoon in listOfDoc)
            {
                if (titleGoodNightMoon.title.Equals("Goodnight Moon"))
                {
                    book.Add(titleGoodNightMoon.title);
                }
            }

     //3.1.1.	Prints out the total number of books with the title matching exactly [Goodnight Moon] (case sensitive)
            uc.WriteCountFile(book, "BookCount");         
       
           IList<string> listOfKeys = new List<string>();

            foreach (var docs in listOfDoc)
            {
                if (docs.publish_year != null)
                {
                    foreach (var pubYear in docs.publish_year)
                    {
                        if (pubYear >= 2000)
                        {
                            listOfKeys.Add(docs.key);
                        }
                    }
                }
            }

    // 3.1.2 Prints out the list of [key] of books that were published since 2000
            uc.WriteFile(listOfKeys,"ListOfKeys");         
        }

       
        [Theory]
        [InlineData("Sample.json")]
        public void Test2(string fileName)
        {
            var client = new RestClient("https://openlibrary.org");
            var request = new RestRequest("search.json", Method.Get);
            request.AddParameter("title", "Goodnight+Moon+Base");

            var response = client.ExecuteGet(request);
            var deserializedResponse = JsonConvert.DeserializeObject<BookClass>(response.Content);

            string expectedResponse =  uc.GetJsonFile(fileName);         

            var expectedResponseDeserialized = JsonConvert.DeserializeObject<BookClass>(expectedResponse);                        
           
            IList<Doc> listOfDoc = deserializedResponse.docs;      

            foreach (var titleGoodNightMoon in listOfDoc)
            {
                if (titleGoodNightMoon.title.Equals("Goodnight Moon Base"))
                {
                    try
                    {
                        expectedResponseDeserialized.Should().BeEquivalentTo(deserializedResponse);
                    }
                    catch (Exception ex)
                    {
                        IList<string> list = new List<string>();
                        string message = ex.Message;
                        list.Add(message);

     //3.2.1.	Validates whether the response matches the below expected response. If not matched, print out the difference:
                        uc.WriteFile(list, "DifferenceInResponse");
                    }
                }
            }
        }
    }
}

