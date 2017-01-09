using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JsonDotNet.CustomContractResolvers;
using Newtonsoft.Json;

namespace NSC.ZJJCode
{
    public class TB04
    {
        private NSCEntities db = new NSCEntities();

        public object Save(string json)
        {
            using (var dbcon = db.Connection)
            {
                dbcon.Open();
                using (var trans = dbcon.BeginTransaction())
                {
                    try
                    {
                        DT04 dt = JsonConvert.DeserializeObject<DT04>(json);
                        dt.D03 = DateTime.Now;

                        db.DT04.AddObject(dt);

                        var msg = db.SaveChanges();

                        trans.Commit();

                        return msg;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return ex.Message;
                    }
                }
            }
        }

        public string Query(string city_code, string county_code, string station_name)
        {
            try
            {
                IQueryable<DT04> query = db.DT04;

                if (!string.IsNullOrEmpty(county_code))  //市、县查询
                {
                    query = db.DT04.Where(t => t.DD1 == county_code).AsQueryable();
                }
                else if (!string.IsNullOrEmpty(city_code))  //市查询
                {
                    city_code = city_code.Substring(0, 4);
                    query = db.DT04.Where(t => t.DD1.StartsWith(city_code)).AsQueryable();
                }

                if (!string.IsNullOrEmpty(station_name))
                {
                    query = query.Where(t => t.DD2.Contains(station_name)).AsQueryable();
                }

                var settings = getSerializerSettings();
                settings.DateFormatString = "yyyy-MM-dd hh:mm";

                return JsonConvert.SerializeObject(query.ToList(), settings);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string QueryStation(string key_words)
        {
            try
            {
                var list = db.DT04.Where(t => t.DD2.Contains(key_words)).Select(t => t.DD2).ToList();

                return JsonConvert.SerializeObject(list, getSerializerSettings());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ChangeState(int id, string oper)
        {
            try
            {
                int msg = 0;
                switch (oper)
                {
                    case "remove":
                        msg = 1;
                        var dt04 = db.DT04.SingleOrDefault(t => t.D01 == id);
                        db.DT04.DeleteObject(dt04);
                        //尚未删除Sys表
                        break;
                    case "send":
                        msg = 3;
                        db.DT04.SingleOrDefault(t => t.D01 == id).D02 = msg;
                        break;
                    case "record":
                        msg = 4;
                        db.DT04.SingleOrDefault(t => t.D01 == id).D02 = msg;
                        break;
                    case "untread":
                        msg = 2;
                        db.DT04.SingleOrDefault(t => t.D01 == id).D02 = msg;
                        break;
                }

                db.SaveChanges();

                return msg.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void Download(string file_name, string file_url, HttpContextBase context)
        {
            context.Response.AppendHeader("Content-disposition", "attachment;filename=" + file_name);
            context.Response.WriteFile(AppDomain.CurrentDomain.BaseDirectory + file_url);
        }

        private JsonSerializerSettings getSerializerSettings()
        {
            var serializerSettings = new JsonSerializerSettings();
            var propertiesContractResolver = new PropertiesContractResolver();

            propertiesContractResolver.ExcludeProperties.Add("$id");
            propertiesContractResolver.ExcludeProperties.Add("EntityKey");

            serializerSettings.ContractResolver = propertiesContractResolver;

            return serializerSettings;
        }

        public object Template_Save()
        {
            using (var dbcon = db.Connection)
            {
                dbcon.Open();
                using (var trans = dbcon.BeginTransaction())
                {
                    try
                    {
                        trans.Commit();

                        return null;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return ex.Message;
                    }
                }
            }
        }

        public object Template_Query()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}