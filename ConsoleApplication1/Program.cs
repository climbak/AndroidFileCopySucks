using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
  class Program
  {
    static void Main(string[] args)
    {
      //DateTime date;
      //using (FileStream fs = new FileStream("C:\\Users\\ryan7423\\Pictures\\ryan\\temp\\20151206_170331.jpg", FileMode.Open, FileAccess.Read))
      //using (Image myImage = Image.FromStream(fs, false, false))
      //{
      //  PropertyItem propItem = myImage.GetPropertyItem(36867);
      //  string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
      //  date = DateTime.Parse(dateTaken);
      //}

      //File.SetCreationTime("C:\\Users\\ryan7423\\Pictures\\ryan\\temp\\20151206_170331.jpg", date);

      var dirPath = "f:\\Download";
      Directory.SetCurrentDirectory(dirPath);

      var dirInfo = new DirectoryInfo(dirPath);

      var dirs = dirInfo.GetDirectories();

      //foreach (var dir in dirs)
      //{
      //  var name = dir.Name;
      //  if (String.Equals(name, "DCIM") /*|| String.Equals(name, "Download") || String.Equals(name, "Pictures") || String.Equals(name, "PlayMemories Mobile") || String.Equals(name, "SdCardBackUp")*/)
      UpdateTimes(dirInfo);
      //}

      using (StreamWriter w = File.AppendText("C:\\Users\\ryan7423\\Desktop\\log.txt"))
      {
        w.WriteLine(sb.ToString());
        w.WriteLine("Total files: " + count);
      }
    }

    private static Regex r = new Regex(":");
    private static StringBuilder sb = new StringBuilder();
    private static int count = 0;

    static void UpdateTimes(DirectoryInfo dir)
    {
      foreach (var d in dir.GetDirectories())
      {
        UpdateTimes(d);
      }

      foreach (var file in dir.GetFiles())
      {
        count++;
        DateTime date = DateTime.Now;
        bool go = false;
        var ext = file.Extension.ToLower();
        if (ext.Contains("jpg") || ext.Contains("jpeg") || ext.Contains("gif") || ext.Contains("dng") || ext.Contains("tif") || ext.Contains("png"))
        {
          try
          {
            using (FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
              try
              {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                date = DateTime.Parse(dateTaken);
                go = true;
              }
              catch (Exception e)
              {
                //sb.AppendLine(file.FullName + "; " + e.Message);
              }
            }
          }
          catch (Exception e) { }

          if (!go)
          {
            try
            {
              var name = new String(file.Name.Where(Char.IsDigit).ToArray()).Substring(0, 14);
              if (DateTime.TryParseExact(name, new string[] { "yyyyMMddHHmmss", "ddMMyyyyHHmmss" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
              {
                if (date.Year <= DateTime.Now.Year && date.Year > 2000)
                  go = true;
              }
            }
            catch (Exception e)
            {
              sb.AppendLine(file.FullName + "; " + e.Message);
            }
          }
        }
        else if (!go && (ext.Contains("avi") || ext.Contains("mov") || ext.Contains("mp4") || ext.Contains("mpeg") || ext.Contains("3gp") || ext.Contains("mkv")))
        {
          try
          {
            var name = new String(file.Name.Where(Char.IsDigit).ToArray()).Substring(0, 14);
            if (DateTime.TryParseExact(name, new string[] { "yyyyMMddHHmmss", "ddMMyyyyHHmmss" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
            {
              if (date.Year <= DateTime.Now.Year && date.Year > 2000)
                go = true;
            }
          }
          catch (Exception e)
          {
            sb.AppendLine(file.FullName + "; " + e.Message);
          }
        }

        if (go)
        {
          try
          {
            if (file.CreationTime != date)
              file.CreationTime = date;
          }
          catch (Exception e)
          {
            sb.AppendLine(file.FullName + "; " + e.Message);
          }
        }
        else
        {
          try
          {
            file.CreationTime = file.CreationTime.AddYears(-2);
          }
          catch (Exception e)
          {
            sb.AppendLine(file.FullName + "; " + e.Message);
          }
        }
      }
    }
  }
}
