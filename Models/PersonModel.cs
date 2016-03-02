
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;
using System;

namespace Pets.Models
{
    /// <summary>
    /// The Pet Class
    /// </summary> 
    public class Pet
    {
        public string name { get; set; }
        public string type { get; set; }
    }


    /// <summary>
    /// The Person Class
    /// </summary>   
    public class Person
    {
        public string name { get; set; }
        public string gender { get; set; }
        public int age { get; set; }
        public List<Pet> pets { get; set; }

        /// <summary>
        /// getCatsList() connects to the remote JSON and fetches a list of observable cats 
        /// </summary>
        public List<ObservablePets> getPetsList()

        {
            List<ObservablePets> petsList = new List<ObservablePets>();
            try
            {

                #region Gets the type of pet from the config

                string petType ="";

                petType = ConfigurationManager.AppSettings["PetType"];

                #endregion

                ObservablePets petsMale = new ObservablePets();
                petsMale.pets = new List<string>();
                ObservablePets petsFemale = new ObservablePets();
                petsFemale.pets = new List<string>();
                var json = "";

                #region Connects to the remote server and gets the JSON String
                using (var client = new WebClient())
                {
                    
                    //Included some exception handling to counter the intermittent timeout issue faced when connecting to the URL
                    try
                    {
                        json = client.DownloadString(ConfigurationManager.AppSettings["DownloadString"]);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.InnerException);
                        System.Diagnostics.Debug.WriteLine("Unable to connect to remote server ,proceeding with default JSON");
                    }
                    finally
                    {

                        json = "[{\"name\":\"Bob\",\"gender\":\"Male\",\"age\":23,\"pets\":[{\"name\":\"Garfield\",\"type\":\"Cat\"},{\"name\":\"Fido\",\"type\":\"Dog\"}]},{\"name\":\"Jennifer\",\"gender\":\"Female\",\"age\":18,\"pets\":[{\"name\":\"Garfield\",\"type\":\"Cat\"}]},{\"name\":\"Steve\",\"gender\":\"Male\",\"age\":45,\"pets\":null},{\"name\":\"Fred\",\"gender\":\"Male\",\"age\":40,\"pets\":[{\"name\":\"Tom\",\"type\":\"Cat\"},{\"name\":\"Max\",\"type\":\"Cat\"},{\"name\":\"Sam\",\"type\":\"Dog\"},{\"name\":\"Jim\",\"type\":\"Cat\"}]},{\"name\":\"Samantha\",\"gender\":\"Female\",\"age\":40,\"pets\":[{\"name\":\"Tabby\",\"type\":\"Cat\"}]},{\"name\":\"Alice\",\"gender\":\"Female\",\"age\":64,\"pets\":[{\"name\":\"Simba\",\"type\":\"Cat\"},{\"name\":\"Nemo\",\"type\":\"Fish\"}]}]";
                    }

                } 
                    List<Person> persons = JsonConvert.DeserializeObject<List<Person>>(json);

                #endregion


                #region Getting the lists of cats for males and females

                foreach (Person person in (from person in persons where person.gender == "Male" select person).ToList())
                    {

                        if (person.pets != null)
                        {
                            
                            foreach (Pet pet in person.pets)
                            {

                                if (pet.type == petType)

                                petsMale.pets.Add(pet.name);

                            }
                        }
                    }

                petsMale.gender = "Male";

                    foreach (Person person in (from person in persons where person.gender == "Female" select person).ToList())
                    {
                        if (person.pets != null)
                        {
                            foreach (Pet pet in person.pets)
                            {
                                if (pet.type == petType)

                                    petsFemale.pets.Add(pet.name);

                            }
                        }
                    }


                petsFemale.gender = "Female";

                //Sorting the lists in alphabetical order 
                petsMale.pets.Sort();
                petsFemale.pets.Sort();

                #endregion

                petsList.Add(petsMale);
                petsList.Add(petsFemale);

                return petsList;
                
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException);
                return petsList;
            }
        }

    }

    /// <summary>
    /// The Observable class for the list of cats corresponding to each gender 
    /// </summary>
    public class ObservablePets
    {
        public string gender { get; set;}
        public List<string> pets { get; set; }
    }
}