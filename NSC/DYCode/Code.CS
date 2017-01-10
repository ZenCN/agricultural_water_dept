using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dynashety.Plug;
using ElegantWM.Tools;
using System.IO;
using System.Web.Helpers;
using Dynashety;
using Dynashety.DYAttribute;
using Newtonsoft.Json;

namespace NSC.DYCode
{
    public class UpLoad_DataBase : I_FileUpLoad
    {
        private NSCEntities db = new NSCEntities();

        private Table tb;
        private SX02_USER s;

        private string basePath = System.Configuration.ConfigurationManager.AppSettings["dirPath"];

        public UpLoad_DataBase(Table tb, SX02_USER s)
        {
            this.s = s;
            this.tb = tb;
        }


        //表4单文件上传
        public string Handle(string md5, HttpPostedFileBase file)
        {
            string msg = "";

            using (var dbcon = db.Connection)
            {
                dbcon.Open();
                using (var trans = dbcon.BeginTransaction())
                {
                    SX04_SYS sx04 = null;
                    try
                    {
                        string guid = Guid.NewGuid().ToString("N");
                        sx04 = new SX04_SYS();
                        sx04.MD5 = Tool.getMd5(file.InputStream);
                        sx04.FILEURL = basePath + Path.DirectorySeparatorChar + guid;
                        sx04.FILEPATH = AppDomain.CurrentDomain.BaseDirectory + basePath + Path.DirectorySeparatorChar +
                                        guid;

                        file.SaveAs(sx04.FILEPATH);

                        db.SX04_SYS.AddObject(sx04);

                        db.SaveChanges();

                        trans.Commit();

                        return JsonConvert.SerializeObject(new { file_name = file.FileName, file_path = guid });
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();

                        if (File.Exists(sx04.FILEPATH))
                        {
                            File.Delete(sx04.FILEPATH);
                        }

                        msg = ex.Message;
                    }
                }
            }

            return msg;
        }

        //多文件上传
        public List<RetFileInfo> Handle(List<UFileInfo> list, List<HttpPostedFileBase> files)
        {
            SX04_SYS file = null;
            var retList = new List<RetFileInfo>();
            var delList = new List<SX04_SYS>();
            using (var dbcon = db.Connection)
            {
                dbcon.Open();
                using (var BT = dbcon.BeginTransaction())
                {
                    try
                    {
                        (list ?? new List<UFileInfo>()).ForEach(s =>
                        {
                            file = db.SX04_SYS.SingleOrDefault(t => t.MD5 == s.md5);
                            retList.Add(new RetFileInfo
                            {
                                fileName = s.fileName,
                                fileUrl = file.FILEURL,
                                contentType = string.Empty,
                                contentLength = -1
                            });
                        });
                        files.Where(t => t != null).ToList().ForEach(f =>
                        {
                            var md5 = Tool.getMd5(f.InputStream);
                            file = db.SX04_SYS.SingleOrDefault(t => t.MD5 == md5);
                            if (file == null)
                            {
                                var fileName = Guid.NewGuid().ToString("N");
                                file = new SX04_SYS
                                {
                                    MD5 = md5,
                                    FILEURL = basePath + Path.DirectorySeparatorChar + fileName,
                                    FILEPATH = AppDomain.CurrentDomain.BaseDirectory + basePath + Path.DirectorySeparatorChar + fileName
                                };
                                f.SaveAs(file.FILEPATH);
                                delList.Add(file);
                                db.SX04_SYS.AddObject(file);
                            }
                            retList.Add(new RetFileInfo
                            {
                                fileName = f.FileName,
                                fileUrl = file.FILEURL,
                                contentType = f.ContentType,
                                contentLength = f.ContentLength
                            });
                        });
                        retList.ForEach(f =>
                        {
                            switch (tb)
                            {
                                case Table.DT01:
                                    db.DT01.AddObject(new DT01
                                    {
                                        D02 = f.fileName,
                                        D03 = DateTime.Now,
                                        D04 = f.fileUrl,
                                        D05 = s.NAME
                                    });
                                    break;
                                case Table.DT02:
                                    db.DT02.AddObject(new DT02
                                    {
                                        D02 = f.fileName,
                                        D03 = DateTime.Now,
                                        D04 = f.fileUrl,
                                        D05 = s.NAME
                                    });
                                    break;
                                case Table.DT03:
                                    db.DT03.AddObject(new DT03
                                    {
                                        D02 = f.fileName,
                                        D03 = DateTime.Now,
                                        D04 = f.fileUrl,
                                        D05 = s.NAME
                                    });
                                    break;
                                default:
                                    break;
                            }
                        });
                        db.SaveChanges();
                        BT.Commit();
                        return retList;
                    }
                    catch (Exception e)
                    {
                        BT.Rollback();
                        delList.ForEach(t => File.Delete(t.FILEPATH));
                        throw e;
                    }
                }
            }
        }
    }

    public class Other
    {
        private NSCEntities db = new NSCEntities();

        public List<DFileInfo> DownLoad(int[] list)
        {
            return new NSCEntities().DT01.Where(t => t.D99 == 0 && list.Contains(t.D01)).Select(t => new DFileInfo { fileName = t.D02, fileUrl = t.D04 }).ToList();
        }

        public object Serach(int year, string name, Table tb)
        {
            switch (tb)
            {
                case Table.DT01:
                    return db.DT01.Where(t => t.D99 == 0 && t.D03.Value.Year == year && (string.IsNullOrEmpty(name) ? true : t.D02.Contains(name)))
                        .Select(t => t.D02).ToList();
                case Table.DT02:
                    return db.DT02.Where(t => t.D99 == 0 && t.D03.Value.Year == year && (string.IsNullOrEmpty(name) ? true : t.D02.Contains(name)))
                        .Select(t => t.D02).ToList();
                case Table.DT03:
                    return db.DT03.Where(t => t.D99 == 0 && t.D03.Value.Year == year && (string.IsNullOrEmpty(name) ? true : t.D02.Contains(name)))
                        .Select(t => t.D02).ToList();
                default:
                    return null;
            }
        }

        public object Index(int year, string name, Table tb)
        {
            switch (tb)
            {
                case Table.DT01:
                    return db.DT01.Where(t => t.D99 == 0 && t.D03.Value.Year == year 
                        && (string.IsNullOrEmpty(name) ? true : t.D02.Contains(name)))
                        .Select(t => new { D01 = t.D01, D02 = t.D02, D03 = t.D03 }).ToList();
                case Table.DT02:
                    return db.DT02.Where(t => t.D99 == 0 && t.D03.Value.Year == year && (string.IsNullOrEmpty(name) ? true : t.D02.Contains(name)))
                        .Select(t => new { D01 = t.D01, D02 = t.D02, D03 = t.D03 }).ToList();
                case Table.DT03:
                    return db.DT03.Where(t => t.D99 == 0 && t.D03.Value.Year == year && (string.IsNullOrEmpty(name) ? true : t.D02.Contains(name)))
                        .Select(t => new { D01 = t.D01, D02 = t.D02, D03 = t.D03 }).ToList();
                default:
                    return null;
            }
        }

        public object Delete(int index, Table tb)
        {
            switch (tb)
            {
                case Table.DT01:
                    db.DT01.SingleOrDefault(t => t.D01 == index && t.D99 == 0).D99 = 1;
                    return db.SaveChanges();
                case Table.DT02:
                    db.DT02.SingleOrDefault(t => t.D01 == index && t.D99 == 0).D99 = 1;
                    return db.SaveChanges();
                case Table.DT03:
                    db.DT03.SingleOrDefault(t => t.D01 == index && t.D99 == 0).D99 = 1;
                    return db.SaveChanges();
                default:
                    return null;
            }
        }
    }

    public enum Table { DT01, DT02, DT03 , DT04}
}