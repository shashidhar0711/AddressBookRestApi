using AddressBookRestApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace AddressBookRestApiTest
{
    [TestClass]
    public class AddressBookTest
    {
        RestClient client;
        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
        }

        /// <summary>
        /// UC22
        /// Called when [calling list return contact list].
        /// </summary>
        [TestMethod]
        public void OnCallingList_ReturnContactList()
        {
            IRestResponse response = GetEmployeeList();
            /// Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<AddressBookModel> dataResponse = JsonConvert.DeserializeObject<List<AddressBookModel>>(response.Content);
            Assert.AreEqual(4, dataResponse.Count);
            foreach (AddressBookModel contact in dataResponse)
            {
                Console.WriteLine("Id:" + contact.Id + "\nName:" + contact.Name + "\nAddress:" + contact.Address + "\nPhoneNumber: "+contact.PhoneNumber, "\nEmail:"+contact.Email);
            }
        }

        /// <summary>
        /// Using Interface to get all list of employees.
        /// </summary>
        /// <returns></returns>
        public IRestResponse GetEmployeeList()
        {
            RestRequest request = new RestRequest("/contact", Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }
    }
}
