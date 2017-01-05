using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace NSC.ZJJCode
{
    public class Handle
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

        public object Template()
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
    }
}