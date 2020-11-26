﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qnify.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Qnify.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpPost("UploadFiles")]
        public async Task<ActionResult<ImageResponse>> Post([FromForm] List<IFormFile> files)
        {
            var result = new ImageResponse();

            try
            {
                foreach (var formFile in files)
                {
                    //file name
                    var fileName = $@"{Guid.NewGuid().ToString().ToUpper()}.{formFile.FileName.Split(".")[1]}";

                    var ipAddress = GetLocalIPAddress();

                    // full path to file in temp location     
                    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                    result.CellImage.Add(filePath);

                    if (formFile.Length > 0)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        //internal bool Upload(string relativePath, HttpPostedFileBase file, string saveAsName)
        //{
        //    try
        //    {
        //        string targetFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        //        string fileName = saveAsName;
        //        string targetPath = Path.Combine(targetFolder, fileName);
        //        file.SaveAs(targetPath);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message, ex);
        //        return false;
        //    }
        //}

        //public ActionResult GetUploadReceipt(string imageName)
        //{
        //    string extName = Path.GetExtension(imageName);
        //    string relativePath = Path.Combine(ConfigurationManager.AppSettings["UploadReceiptPhotoPath"], imageName);
        //    string fullFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        //    return base.File(fullFilePath, "image/" + extName, imageName);
        //}

        //public ActionResult GetEditorImage(string image)
        //{
        //    string extName = Path.GetExtension(image);
        //    string relativePath = Path.Combine(ConfigurationManager.AppSettings["EditorImagesPath"], image);
        //    string fullFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        //    return base.File(fullFilePath, "image/" + extName, image);
        //}

        ////Upload from quill image handler
        //public JsonResult UploadEditorImage(HttpPostedFileWrapper image)
        //{
        //    string relativePath = ConfigurationManager.AppSettings["EditorImagesPath"];

        //    Upload(relativePath, image, image.FileName);

        //    return Json(new { data = Url.Action("GetEditorImage", "Image", new { image = image.FileName }) }, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult ImageWithSubFolder(string imagename, string subfolder)
        //{
        //    string photorootpath = ConfigurationManager.AppSettings["PhotoUploadPath"];
        //    if (String.IsNullOrEmpty(photorootpath))
        //        throw new Exception("PhotoRootPath is not define in Web Config"); //
        //    string photofullpath = photorootpath + "\\" + subfolder + "\\" + imagename;
        //    return base.File(photofullpath, "image/jpeg", imagename);

        //}

        //public ActionResult ImageDefaultPath(string imagename)
        //{
        //    string photorootpath = ConfigurationManager.AppSettings["PhotoUploadPath"];
        //    if (String.IsNullOrEmpty(photorootpath))
        //        throw new Exception("PhotoRootPath is not define in Web Config"); //
        //    string photofullpath = photorootpath + "\\" + imagename;
        //    return base.File(photofullpath, "image/jpeg", imagename);

        //}

        //public ActionResult ImageWithThumnbail(string DistriCode, string SalesmanCode, string imagename, string subfolder, int width, int height)
        //{
        //    Image img = null;
        //    try
        //    {
        //        string photorootpath = ConfigurationManager.AppSettings["PhotoUploadPath"];
        //        if (String.IsNullOrEmpty(photorootpath))
        //            throw new Exception("PhotoRootPath is not define in Web Config"); //
        //        string imagepath = photorootpath + "\\" + DistriCode + "\\" + SalesmanCode + "\\" + imagename.Trim();
        //        img = System.Drawing.Image.FromFile(imagepath);
        //        if (img == null)
        //        {
        //            throw new ArgumentNullException("Image");
        //        }
        //        var ms = GetMemoryStream(img, width, height, ImageFormat.Jpeg);

        //        // output
        //        Response.Clear();
        //        Response.ContentType = "image/jpeg";

        //        //var a = System.Drawing.Image.Save(ms, ImageFormat.Jpeg);
        //        return File(ms, Response.ContentType);
        //    }
        //    catch (Exception ex)
        //    {
        //        //img = new Bitmap(1, 1);
        //        return null;
        //    }
        //    finally
        //    {
        //        if (img != null) img.Dispose();
        //    }
        //}

        //public ActionResult ImageWithSubFolderThumnbail(string imagename, string subfolder, int width, int height)
        //{
        //    Image img = null;
        //    try
        //    {
        //        string photorootpath = ConfigurationManager.AppSettings["PhotoUploadPath"];
        //        if (String.IsNullOrEmpty(photorootpath))
        //            throw new Exception("PhotoRootPath is not define in Web Config"); //
        //        string imagepath = photorootpath + "\\" + subfolder + "\\" + imagename;
        //        img = System.Drawing.Image.FromFile(imagepath);
        //        if (img == null)
        //        {
        //            throw new ArgumentNullException("Image");
        //        }
        //        var ms = GetMemoryStream(img, width, height, ImageFormat.Jpeg);

        //        // output
        //        Response.Clear();
        //        Response.ContentType = "image/jpeg";

        //        //var a = System.Drawing.Image.Save(ms, ImageFormat.Jpeg);
        //        return File(ms, Response.ContentType);
        //    }
        //    catch (Exception ex)
        //    {
        //        //img = new Bitmap(1, 1);
        //        return null;
        //    }
        //    finally
        //    {
        //        if (img != null) img.Dispose();
        //    }
        //}


        //private static MemoryStream GetMemoryStream(Image input, int width, int height, ImageFormat fmt)
        //{
        //    // maintain aspect ratio
        //    if (input.Width > input.Height)
        //        height = input.Height * width / input.Width;
        //    else
        //        width = input.Width * height / input.Height;

        //    var bmp = new Bitmap(input, width, height);
        //    var ms = new MemoryStream();
        //    bmp.Save(ms, ImageFormat.Jpeg);
        //    ms.Position = 0;
        //    return ms;
        //}
    }
}