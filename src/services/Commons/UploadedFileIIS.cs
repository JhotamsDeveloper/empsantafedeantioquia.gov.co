using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace services.Commons
{
    public interface IUploadedFileIIS
    {
        string UploadedFileImage(string value, IFormFile file, string account, bool isCoverage);
        string UploadedFileImage(IFormFile value, string account);
        List<string> UploadedMultipleFileImage(IEnumerable<IFormFile> files, string account);
        List<string> UploadedMultipleFileImage(IEnumerable<IFormFile> files, List<string> value, string account);
        bool UploadedMultipleFileImage(List<string> value, string account);
        Boolean DeleteConfirmed(string imgModel, string account);
        string UploadedFileImage(IFormFile value, string namefile, string account);
    }
    public class UploadedFileIIS : IUploadedFileIIS
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadedFileIIS(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string UploadedFileImage(IFormFile file, string account)
        {
            string uniqueFileName;

            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images\\" + account);
            uniqueFileName = "images-" + Guid.NewGuid().ToString() + "." + Path.GetExtension(file.FileName).Substring(1);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        public string UploadedFileImage(string value, IFormFile file, string account, bool isCoverage)
        {
            string uniqueFileName = null;

            if (value != null && file != null)
            {
                var _deleteFile = DeleteUpload(value, account);
            }

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images\\" + account);

                if (isCoverage)
                {
                    uniqueFileName = "Coverpague" + "." + Path.GetExtension(file.FileName).Substring(1);
                }
                else
                {
                    uniqueFileName = "images-" + Guid.NewGuid().ToString() + "." + Path.GetExtension(file.FileName).Substring(1);
                }

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);

            }
            return uniqueFileName;
        }

        public List<string> UploadedMultipleFileImage(IEnumerable<IFormFile> files, string account)
        {
            //Nombre de archivo único
            List<string> uniqueFileName = new List<string>();
            int _contador = 0;

            foreach (var file in files)
            {
                _contador++;
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images\\" + account);
                uniqueFileName.Add("images-" + Guid.NewGuid().ToString() + "." + Path.GetExtension(file.FileName).Substring(1));
                string filePath = Path.Combine(uploadsFolder, uniqueFileName[_contador - 1]);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        public List<string> UploadedMultipleFileImage(IEnumerable<IFormFile> files, List<string> value, string account)
        {
            //Nombre de archivo único
            List<string> uniqueFileName = new List<string>();

            if (value.Count != 0)
            {
                DeleteUpload(value, account);
            }

            if (files != null)
            {

                int _contador = 0;

                foreach (var file in files)
                {
                    _contador++;
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images\\" + account);
                    uniqueFileName.Add("images-" + Guid.NewGuid().ToString() + "." + Path.GetExtension(file.FileName).Substring(1));
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName[_contador - 1]);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
            }

            return uniqueFileName;
        }

        public Boolean DeleteConfirmed(string imgModel, string account)
        {
            return DeleteUpload(imgModel, account);
        }

        private Boolean DeleteUpload(string imgModel, string account)
        {

            imgModel = Path.Combine(_hostingEnvironment.WebRootPath, "images\\" + account, imgModel);
            FileInfo fileInfo = new FileInfo(imgModel);

            if (fileInfo != null)
            {
                System.IO.File.Delete(imgModel);
                fileInfo.Delete();

                return true;
            }
            else
            {
                return false;
            }
        }

        private Boolean DeleteUpload(List<string> imgModel, string account)
        {
            for (int i = 0; i < imgModel.Count; i++)
            {
                imgModel[i] = Path.Combine(_hostingEnvironment.WebRootPath, $"images\\{account}", imgModel[i]);
                FileInfo fileInfo = new FileInfo(imgModel[i]);

                if (fileInfo != null)
                {
                    System.IO.File.Delete(imgModel[i]);
                    fileInfo.Delete();

                }

            }

            return true;
        }

        public bool UploadedMultipleFileImage(List<string> value, string account)
        {
            Boolean _status = false;

            if (value.Count != 0)
            {
                DeleteUpload(value, account);
                _status = true;
            }

            return _status;
        }

        public string UploadedFileImage(IFormFile file, string namefile, string account)
        {
            string uniqueFileName;

            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images\\" + account);
            uniqueFileName = namefile + "." + Path.GetExtension(file.FileName).Substring(1);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}
