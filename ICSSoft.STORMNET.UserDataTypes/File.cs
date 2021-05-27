// -----------------------------------------------------------------------
// <copyright file="File.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using ICSharpCode.SharpZipLib.Zip;

namespace ICSSoft.STORMNET.FileType
{
    using System;
    using System.IO;

    using ICSharpCode.SharpZipLib.Core;

    using ICSSoft.STORMNET.Windows.Forms.Binders;

    /// <summary>
    /// Тип данных File.
    /// </summary>
    [ControlProvider("ICSSoft.STORMNET.FileType.FileControlProvider, ICSSoft.STORMNET.Windows.Forms")]
    [StoreInstancesInType(typeof(Business.SQLDataService), typeof(string))]
    [Serializable]
    public class File : IConvertible, IFormattable, IComparableType
    {
        private string _zippedValue;

        private string _name;

        private long _size;

        private string _path;

        private int _compressionLevel;

        /// <summary>
        /// Конструктор класса. Ничего не делает.
        /// </summary>
        public File()
        {
        }

        /// <summary>
        /// Из типа File в тип byte[].
        /// </summary>
        /// <param name="value"> Что нужно преобразовать. </param>
        /// <returns> Преобразованное значение. </returns>
        public static explicit operator string(ICSSoft.STORMNET.FileType.File value)
        {
            return value._zippedValue;
        }

        /// <summary>
        /// Из типа byte[] в тип File.
        /// </summary>
        /// <param name="value"> Что нужно преобразовать. </param>
        /// <returns> Преобразованное значение. </returns>
        public static explicit operator File(string value)
        {
            File file = new File();
            file.InitializeByState(value);

            return file;
        }

        /// <summary>
        /// Инициализация по сериализованному значению.
        /// </summary>
        /// <param name="value"> Сериализованное состояние. </param>
        public void InitializeByState(string value)
        {
            byte[] byteValue = Convert.FromBase64String(value);

            using (MemoryStream memoryStream = new MemoryStream(byteValue, true))
            {
                memoryStream.Position = 0;
                using (ZipInputStream zipInputStream = new ZipInputStream(memoryStream))
                {
                    ZipEntry entry = zipInputStream.GetNextEntry();
                    this._zippedValue = value;
                    this.Name = entry.Name;
                    this.Size = entry.Size;
                }
            }
        }

        /// <summary>
        /// Gets or sets: Имя выбранного файла.
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }

            set
            {
                this._name = value;
            }
        }

        /// <summary>
        /// Gets or sets: Сархивированное значение.
        /// </summary>
        public string ZippedValue
        {
            get
            {
                return this._zippedValue;
            }

            set
            {
                this._zippedValue = value;
            }
        }

        /// <summary>
        ///  Gets or sets: Размер выбранного файла.
        /// </summary>
        public long Size
        {
            get
            {
                return this._size;
            }

            set
            {
                this._size = value;
            }
        }

        /// <summary>
        ///  Gets or sets: Степень сжатия изменяется от 0 до 9 (9 - максимальная).
        /// </summary>
        public int CompressionLevel
        {
            get
            {
                return this._compressionLevel;
            }

            set
            {
                this._compressionLevel = value;
            }
        }

        /// <summary>
        /// Перевод с файла на диске во внутреннее представление в виде zip-архива.
        /// </summary>
        /// <param name="loadFilePath"> Путь к файлу.</param>
        public void FromNormalToZip(string loadFilePath)
        {
            var fileStream = new FileStream(loadFilePath, FileMode.Open, FileAccess.Read);
            FromNormalToZip_BasePart(fileStream, System.IO.Path.GetFileName(fileStream.Name));
            fileStream.Close();
        }

        /// <summary>
        /// Перевод с файла на диске во внутреннее представление в виде zip-архива.
        /// </summary>
        /// <param name="loadFilePath"> Путь к файлу.</param>
        /// <param name="fileName"> Имя файла.</param>
        public void FromNormalToZip(string loadFilePath, string fileName)
        {
            var fileStream = new FileStream(loadFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            FromNormalToZip_BasePart(fileStream, fileName);
            fileStream.Close();
        }

        public void FromNormalToZip_BasePart(Stream fileStream, string fileName)
        {
            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);
            zipStream.SetLevel(this.CompressionLevel);
            ZipEntry newEntry = new ZipEntry(fileName);

            zipStream.PutNextEntry(newEntry);
            StreamUtils.Copy(fileStream, zipStream, new byte[4096]);
            zipStream.CloseEntry();

            zipStream.IsStreamOwner = false;             // False stops the Close also Closing the underlying stream.
            zipStream.Close();                                             // Must finish the ZipOutputStream before using outputMemStream.

            outputMemStream.Position = 0;
            byte[] fileData = new byte[outputMemStream.Length];
            outputMemStream.Read(fileData, 0, (int)outputMemStream.Length);

            this._zippedValue = Convert.ToBase64String(fileData);
            outputMemStream.Close();
        }

        /// <summary>
        /// Перевод с файла на диске во внутреннее представление в виде zip-архива.
        /// </summary>
        /// <param name="memoryStream"> Stream с файлом. </param>
        public void SetZippedValue(MemoryStream memoryStream)
        {
            if (memoryStream != null)
            {
                FromNormalToZip_BasePart(memoryStream, new Guid().ToString());
            }
        }

        /// <summary>
        /// Перевод из внутреннего представления в виде zip-архива в файл на диске.
        /// </summary>
        /// <param name="saveFilePath"> Путь к файлу.</param>
        public bool FromZipToNormal(string saveFilePath)
        {
            bool result;
            FileStream fileStream = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write);
            result = FromZipToNormal_BasePart(fileStream);
            fileStream.Close();
            if (!result)
            {
                System.IO.File.Delete(saveFilePath);
            }

            return result;
        }

        /// <summary>
        /// Перевод из внутреннего представления в виде zip-архива в файл на диске.
        /// </summary>
        /// <returns> Stream с содержимым файла (без zip-архивации). </returns>
        public MemoryStream GetUnzippedFile()
        {
            MemoryStream memoryStream = new MemoryStream();
            FromZipToNormal_BasePart(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }

        private bool FromZipToNormal_BasePart(Stream outputStream)
        {
            byte[] byteValue = Convert.FromBase64String(_zippedValue);

            MemoryStream memoryStream = new MemoryStream(byteValue, true);

            memoryStream.Position = 0;
            ZipInputStream zipStream = new ZipInputStream(memoryStream);
            bool isZip = true;
            try
            {
                ZipEntry zipEntry = zipStream.GetNextEntry();
                if (zipEntry == null)
                {
                    isZip = false;
                }
            }
            catch (Exception)
            {
                isZip = false;
            }

            if (isZip)
            {
                StreamUtils.Copy(zipStream, outputStream, new byte[4096]);
            }
            else
            {
                throw new Exception("Ошибка работы с файлом.");
            }

            zipStream.Close();
            memoryStream.Close();
            return isZip;
        }

        /// <summary>
        /// Записать файл в массив байт.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        public void GetFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            this.Size = fileInfo.Length;
            this.Name = fileInfo.Name;

            using (FileStream fsSource = new FileStream(path,
            FileMode.Open, FileAccess.Read))
            {
                byte[] readed = new byte[fsSource.Length];
                int length = (int)fsSource.Length;

                fsSource.Read(readed, 0, length);

                _zippedValue = Convert.ToBase64String(readed);
            }
        }

        #region IConvertible members

        public TypeCode GetTypeCode()
        {
            return 0;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return false;
        }

        public char ToChar(IFormatProvider provider)
        {
            return '\0';
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return 0;
        }

        public byte ToByte(IFormatProvider provider)
        {
            return 0;
        }

        public short ToInt16(IFormatProvider provider)
        {
            return 0;
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return 0;
        }

        public int ToInt32(IFormatProvider provider)
        {
            return 0;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return 0;
        }

        public long ToInt64(IFormatProvider provider)
        {
            return 0;
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return 0;
        }

        public float ToSingle(IFormatProvider provider)
        {
            return 0;
        }

        public double ToDouble(IFormatProvider provider)
        {
            return 0;
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return 0;
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return DateTime.Now;
        }

        public string ToString(IFormatProvider provider)
        {
            return this.Name;
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return 0;
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        public int Compare(object x)
        {
            if (x == null)
            {
                return -1;
            }

            if (x is File)
            {
                var file = x as File;
                if (file._zippedValue == _zippedValue)
                {
                    return 0;
                }
            }

            return -1;
        }
    }
}
