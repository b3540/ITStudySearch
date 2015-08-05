﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ITStudySearch.WinPhone81;
using Windows.Storage;
using System.IO;

[assembly: Dependency(typeof(SaveAndLoad_WinPhone81))]

namespace ITStudySearch.WinPhone81
{
    class SaveAndLoad_WinPhone81 : ISaveAndLoad
    {
        public string LoadData(string filename)
        {
            var task = LoadDataAsync(filename);
            //task.Wait(); // HACK: to keep Interface return types simple (sorry!)
            //return task.Result;

            #region
            try
            {
                task.Wait(); // HACK: to keep Interface return types simple (sorry!)
                return task.Result;
            }
            catch (AggregateException e)
            {
#if DEBUG
                foreach (var inner in e.InnerExceptions)
                {
                    System.Diagnostics.Debug.WriteLine(inner.Message);
                    System.Diagnostics.Debug.WriteLine("Type : {0}", inner.GetType());
                }
                return null;
#endif
            }
            catch (Exception e)
            {
                throw;
            }
            #endregion

        }
        async Task<string> LoadDataAsync(string filename)
        {
            var local = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                var file = await local.OpenStreamForReadAsync(filename);
                using (StreamReader streamReader = new StreamReader(file))
                {
                    var text = streamReader.ReadToEnd();
                    return text;
                }
            }
            catch (Exception e)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(e.InnerException);
#endif
                return "";
            }

        }
        public async void SaveData(string filename, string text)
        {
            var local = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await local.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            using (StreamWriter writer = new StreamWriter(await file.OpenStreamForWriteAsync()))
            {
                writer.Write(text);
            }
        }

        public bool ClearData(string filename)
        {
            var task = ClearDataAsync(filename);
            task.Wait(); // HACK: to keep Interface return types simple (sorry!)
            return task.Result;
        }

        public async Task<bool> ClearDataAsync(string filename)
        {
            var local = Windows.Storage.ApplicationData.Current.LocalFolder;
            await local.DeleteAsync();

            // ファイルが削除されているか？をGetFileAsyncがFileNotFoundExceptionを返すか？でチェックするのもダサいですよね。。
            try
            {
                var file = await local.GetFileAsync(filename);
            }
            catch (FileNotFoundException)
            {
                return true;
            }
            return false;
        }
    }
}
