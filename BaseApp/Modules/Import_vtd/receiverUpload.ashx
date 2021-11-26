<%@ WebHandler Language="C#" Class="receiverUpload" %>

using System;
using System.Web;
using System.IO;
using System.Web.Hosting;

public class receiverUpload : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string filename = context.Request.QueryString["filename"].ToString();
            using (FileStream fs = File.Create(context.Server.MapPath("~/Upload/UploadedImportVTD/" + filename)))
            {
                SaveFile(context.Request.InputStream, fs);
            }
        }
        catch (Exception e)
        {


        }


        //if (context.Request.InputStream.Length == 0)
        //    throw new ArgumentException("No file input");
        //if (context.Request.QueryString["fileName"] == null)
        //    throw new Exception("Parameter fileName not set!");

        //string fileName = context.Request.QueryString["fileName"];
        //string filePath = context.Server.MapPath("~/Upload/UploadedImportVTD/" + fileName); 
        //// @HostingEnvironment.ApplicationPhysicalPath + "/" + fileName;
        //bool appendToFile = context.Request.QueryString["append"] != null && context.Request.QueryString["append"] == "1";

        //FileMode fileMode;
        //if (!appendToFile)
        //{
        //    if (File.Exists(filePath))
        //        File.Delete(filePath);
        //    fileMode = FileMode.Create;
        //}
        //else
        //{
        //    fileMode = FileMode.Append;
        //}
        //using (FileStream fs = File.Open(filePath, fileMode))
        //{
        //    byte[] buffer = new byte[4096];
        //    int bytesRead;
        //    while ((bytesRead = context.Request.InputStream.Read(buffer, 0, buffer.Length)) != 0)
        //    {
        //        fs.Write(buffer, 0, bytesRead);
        //    }
        //    fs.Close();
        //}
        
    }


  
    
    
    
    
    
    
    
    
    
    
    
    private void SaveFile(Stream stream, FileStream fs)
    {
        byte[] buffer = new byte[4096];
        int bytesRead;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            fs.Write(buffer, 0, bytesRead);
        }
        fs.Close();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
