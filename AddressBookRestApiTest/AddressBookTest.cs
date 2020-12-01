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

        /// <summary>
        /// UC23
        /// Givens the multiple employee on post should return contacts.
        /// </summary>
        [TestMethod]
        public void GivenMultipleContact_OnPost_ShouldReturnContacts()
        {
            /// Arrange
            List<AddressBookModel> addressBookListRestApi = new List<AddressBookModel>();
            addressBookListRestApi.Add(new AddressBookModel { Name = "Varsha", Address = "Kolkatta", PhoneNumber = "9945467618", Email="varsha.varshu@gmail.com" });
            addressBookListRestApi.Add(new AddressBookModel { Name = "Vidhya", Address = "Delhi", PhoneNumber = "9946667618", Email = "vidhya.vid@gmail.com" });
            addressBookListRestApi.ForEach(employeeData =>
            {
                RestRequest request = new RestRequest("/contact", Method.POST);
                JObject jObjectBody = new JObject();
                jObjectBody.Add("Name", employeeData.Name);
                jObjectBody.Add("Address", employeeData.Address);
                jObjectBody.Add("PhoneNumber", employeeData.PhoneNumber);
                jObjectBody.Add("Email", employeeData.Email);

                /// Act
                request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                /// Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                AddressBookModel dataResponse = JsonConvert.DeserializeObject<AddressBookModel>(response.Content);
                Assert.AreEqual(employeeData.Name, dataResponse.Name);
                Assert.AreEqual(employeeData.Address, dataResponse.Address);
                Assert.AreEqual(employeeData.PhoneNumber, dataResponse.PhoneNumber);
                Assert.AreEqual(employeeData.Email, dataResponse.Email);
                Console.WriteLine(response.Content);
            });
            /// Act
            IRestResponse response = GetEmployeeList();
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<AddressBookModel> dataResponse = JsonConvert.DeserializeObject<List<AddressBookModel>>(response.Content);
            /// Assert
            Assert.AreEqual(6, dataResponse.Count);
        }

        /// <summary>
        /// UC24
        /// Givens the employee on update should return updated contact.
        /// </summary>
        [TestMethod]
        public void GivenContact_OnUpdate_ShouldReturnUpdatedContact()
        {
            /// Arrange
            RestRequest request = new RestRequest("/contact/4", Method.PUT);
            JObject jObjectBody = new JObject();
            jObjectBody.Add("name", "Sharath");
            jObjectBody.Add("Address", "AmravathiNagar");
            jObjectBody.Add("PhoneNumber", "9663616724");
            jObjectBody.Add("Email", "sharath.Shanth@gmail.com");
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);
            /// Act
            IRestResponse response = client.Execute(request);
            /// Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            AddressBookModel dataResponse = JsonConvert.DeserializeObject<AddressBookModel>(response.Content);
            Assert.AreEqual("Sharath", dataResponse.Name);
            Assert.AreEqual("AmravathiNagar", dataResponse.Address);
            Assert.AreEqual("9663616724", dataResponse.PhoneNumber);
            Assert.AreEqual("sharath.Shanth@gmail.com", dataResponse.Email);
            Console.WriteLine(response.Content);
        }

        /// <summary>
        /// UC25
        /// Givens the contact identifier on delete should return success status.
        /// </summary>
        [TestMethod]
        public void GivenContactId_OnDelete_ShouldReturnSuccessStatus()
        {
            /// Arrange
            RestRequest request = new RestRequest("/contact/2", Method.DELETE);
            /// Act
            IRestResponse response = client.Execute(request);
            /// Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Console.WriteLine(response.Content);
        }
    }
}
