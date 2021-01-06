using Microsoft.OData.Client;
using ODataUtility.Microsoft.Dynamics.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.OData.Edm;


namespace ODataConsoleApplication
{
    class ODataChangesetsExample
    {
        public static void CreateSalesOrderInSingleChangeset(Resources context)
        {
            string customerGroupId = "10000";
            try
            {
                
                CustomerGroup customerGroup = new CustomerGroup();
                DataServiceCollection<CustomerGroup> customerGroupCollection = new DataServiceCollection<CustomerGroup>(context);
                customerGroupCollection.Add(customerGroup);

                customerGroup.CustomerGroupId = customerGroupId;
                customerGroup.DataAreaId = "USMF";
                customerGroup.PaymentTermId = "KG01";
                customerGroup.TaxGroupId = "KGSTX01";

                
                context.SaveChanges(SaveChangesOptions.PostOnlySetProperties | SaveChangesOptions.BatchWithSingleChangeset); // Batch with Single Changeset ensure the saved changed runs in all-or-nothing mode.
                Console.WriteLine(string.Format("Invoice {0} - Saved !", customerGroupId));
            }
            catch (DataServiceRequestException e)
            {
                Console.WriteLine(string.Format("Invoice {0} - Save Failed !", customerGroupId));
            }
        }
        public static void getDataBase(Resources context)
        {
            SqlConnection connection;
            SqlCommand command;
            string queryString, name ,customerid = "";
            SqlDataReader dataReader;
            string dataareaid = "USMF";
            string connectionString = "Data Source=MININT-5REKJI3;Initial Catalog=WebServices;Integrated Security=True";

            queryString = "select  name , customerid  from dbo.Customer where dataareaid = @Find";

            connection = new SqlConnection(connectionString);

            connection.Open();

            command = new SqlCommand(queryString , connection);

            command.Parameters.Add(new SqlParameter("Find", dataareaid));

            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                Console.WriteLine("{0),{1}", dataReader.GetString(0), dataReader.GetString(1));
                name = dataReader.GetString(0);
;                customerid = dataReader.GetString(1);

            }
            dataReader.Close();
        }
        public static void insertIntoDataBase(Resources context)
        {
            string connectionString, custgroupid = null;

            string identificationnumber = null;

            SqlConnection con;
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd;
            connectionString = "Data Source = MININT - 5REKJI3; Initial Catalog = WebServices; Integrated Security = True";

            con = new SqlConnection(connectionString);
            try
            {
                con.Open();

                Customer customer = new Customer();
                DataServiceCollection<Customer> customerDetailCollection = new DataServiceCollection<Customer>(context);

                customerDetailCollection.Add(customer);

                var CustomerDetailsnew = context.Customers.Where(x => x.DataAreaId == "USMF");

                foreach(var Customer in customerDetailCollection)
                {

                    custgroupid = Customer.CustomerGroupId;
                    identificationnumber = Customer.IdentificationNumber;
                    cmd = new SqlCommand("insert into ax.CustTable (custgroupid,identificationnumber) values (@custgroupid,@IdentificationNumber)");
                    cmd.Parameters.AddWithValue("@custgroupid", custgroupid);
                    cmd.Parameters.AddWithValue("@identificationnumber", identificationnumber);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("identificationNumber", Customer.IdentificationNumber);
                  
                }
                con.Close();
                //console.ReadLine();

                //MessageBox.Show("Rows inserted");

            }
            catch(Exception ex)
            {   
                //MessageBox.Show(ex.ToString());
            }

        }
    }
}
