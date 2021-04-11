using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Areas;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [RoutePrefix("api/Inventory")]
    public class InventoryController : ApiController
    {
        static public string ConnectionString =
            ConfigurationManager.ConnectionStrings["myDb"].ConnectionString;
        SqlConnection con;

        
        [Route("InsertUpdateItemDetails")]
        [HttpPost]
        public IHttpActionResult InsertUpdateItemDetails(Product Pro )
        {
            
            try
            {
                using (con = new SqlConnection(ConnectionString))
                    {
                        con.Open();
                        if (Security.SecurityValidaionAddUpdate(Pro) == "True")
                        {
                        SqlCommand com = new SqlCommand("SP_InsertUpdateItemDetails", con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ItemId", Pro.ItemId);
                        com.Parameters.AddWithValue("@Name", Pro.Name.Trim());
                        com.Parameters.AddWithValue("@Description", Pro.Description.Trim());
                        com.Parameters.AddWithValue("@Price", Pro.Price);
                        var adapter = new SqlDataAdapter(com);
                        //SqlDataReader dr = com.ExecuteReader();
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        List<Product> ProIn = new List<Product>();
                        ProductDetails pr = new ProductDetails();
                        // var productlist = new ObservableCollection<Product>();
                        if (ds != null && ds.Tables.Count == 2)
                        {
                            pr.Output = ds.Tables[0].Rows[0]["OutMasg"].ToString();
                        }
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            var productModel = new Product
                            {
                                ItemId = Convert.ToInt32(ds.Tables[1].Rows[i]["ItemId"].ToString()),
                                Name = ds.Tables[1].Rows[i]["Name"].ToString(),
                                Description = ds.Tables[1].Rows[i]["Description"].ToString(),
                                Price = Convert.ToDecimal(ds.Tables[1].Rows[i]["Price"].ToString())
                            };
                            ProIn.Add(productModel);
                        }

                        pr.ProInfo = ProIn;
                        if (pr != null)
                        {
                            var model = new { Output = pr.Output, pr.ProInfo };
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, model));
                        }
                        else
                        {
                            var model = new { Output = "Execution Failed" };
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, model));
                        }

                        }
                        {
                            var model = new { Output = "Improper Data" };
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, model));
                        }
                    }
                    

            }
            catch (Exception ex)
            {
                var model = new { Output = ex.Message.ToString() };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, model));
            }
            finally
            {
                con.Close();
            }
        }



        [Route("GetRecordsbyId/{ItemId?}")]
        [HttpGet]
        public IHttpActionResult GetRecordsbyId(int ItemId)
        {
            Product Pro = new Product();
            Pro.ItemId = ItemId;
            try
            {
                using (con = new SqlConnection(ConnectionString))
                    {
                        con.Open();
                        if (Security.SecurityValidaionGetDelete(Pro) == "True")
                        {

                         SqlCommand com = new SqlCommand("SP_GetRecordsbyId", con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ItemId", Pro.ItemId);
                        SqlDataReader dr = com.ExecuteReader();

                        List<Product> ProIn = new List<Product>();
                        while (dr.Read())
                        {
                            Pro.ItemId = dr.GetInt32(dr.GetOrdinal("ItemId"));
                            Pro.Name = dr.GetString(dr.GetOrdinal("Name"));
                            Pro.Description = dr.GetString(dr.GetOrdinal("Description"));
                            Pro.Price = dr.GetDecimal(dr.GetOrdinal("Price"));
                        }
                        

                        if (ProIn != null)
                        {
                            var model = new { Output = "success", Pro };
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, model));
                        }
                        else
                        {
                            var model = new { Output = "Execution Failed" };
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, model));
                        }

                    }
                    else
                    {
                        var model = new { Output = "Improper Data" };
                        return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, model));
                    }

                }


            }
            catch (Exception ex)
            {
                var model = new { Output = ex.Message.ToString() };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, model));
            }
            finally
            {
                con.Close();
            }
        }


        [Route("GetAllRecords")]
        [HttpGet]
        public IHttpActionResult GetAllRecords(Product Pro)
        {

            try
            {
                    using (con = new SqlConnection(ConnectionString))
                    {
                        con.Open();

                        SqlCommand com = new SqlCommand("SP_GetAllRecords", con);
                        com.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = com.ExecuteReader();                        
                        List<Product> ProIn = new List<Product>();
                        ProductDetails pr = new ProductDetails();
                   // var productlist = new ObservableCollection<Product>();

                    while (dr.Read())
                    {
                        var productModel = new Product
                        {
                            ItemId = dr.GetInt32(dr.GetOrdinal("ItemId")),
                            Name = dr.GetString(dr.GetOrdinal("Name")),
                            Description = dr.GetString(dr.GetOrdinal("Description")),
                            Price = dr.GetDecimal(dr.GetOrdinal("Price"))
                        };
                        ProIn.Add(productModel);
                       
                    }
                    pr.ProInfo = ProIn;
                    if (ProIn != null)
                        {
                            var model = new { Output = "success", pr.ProInfo };
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, model));
                        }
                        else
                        {
                            var model = new { Output = "Execution Failed" };
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, model));
                        }
                    }
                    
            }
            catch (Exception ex)
            {
                var model = new { Output = ex.Message.ToString() };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, model));
            }
            finally
            {
                con.Close();
            }
        }


        [Route("DeleteItemDetails")]
        [HttpPost]
        public IHttpActionResult DeleteItemDetails(Product Pro)
        {

            try
            {
                    using (con = new SqlConnection(ConnectionString))
                    {
                        con.Open();
                    if (Security.SecurityValidaionGetDelete(Pro) == "True")
                    {

                        SqlCommand com = new SqlCommand("SP_DeleteItemDetails", con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ItemId", Pro.ItemId);
                        var adapter = new SqlDataAdapter(com);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        List<Product> ProIn = new List<Product>();
                        ProductDetails pr = new ProductDetails();
                        if (ds != null && ds.Tables.Count == 2)
                        {
                            pr.Output = ds.Tables[0].Rows[0]["OutMasg"].ToString();
                        }
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            var productModel = new Product
                            {
                                ItemId = Convert.ToInt32(ds.Tables[1].Rows[i]["ItemId"].ToString()),
                                Name = ds.Tables[1].Rows[i]["Name"].ToString(),
                                Description = ds.Tables[1].Rows[i]["Description"].ToString(),
                                Price = Convert.ToDecimal(ds.Tables[1].Rows[i]["Price"].ToString())
                            };
                            ProIn.Add(productModel);
                        }

                        pr.ProInfo = ProIn;
                        if (pr != null)
                        {
                            var model = new { Output = pr.Output, pr.ProInfo };
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, model));
                        }
                        else
                        {
                            var model = new { Output = "Execution Failed" };
                            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, model));
                        }
                    }
                    else
                    {
                        var model = new { Output = "Improper Data" };
                        return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, model));
                    }


                }
            }
            catch (Exception ex)
            {
                var model = new { Output = ex.Message.ToString() };
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, model));
            }
            finally
            {
                con.Close();
            }
        }

        


    }
}
