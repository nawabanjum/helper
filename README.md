  // End #C2MC-5209
        private void exportToJson(string json, string fileName)
        {
            StringBuilder data = new StringBuilder();

            try
            {

                string contentPath = _hostingEnvironment.ContentRootPath;

                string path = Path.Combine(_hostingEnvironment.ContentRootPath, "ElasticSearchData");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(path, fileName + ".json");
                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    sw.WriteLine(json);
                    sw.Close();
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void exportToCSV(DataTable dtData, string fileName)
        {
            StringBuilder data = new StringBuilder();

            //Taking the column names.
            for (int column = 0; column < dtData.Columns.Count; column++)
            {
                //Making sure that end of the line, shoould not have comma delimiter.
                if (column == dtData.Columns.Count - 1)
                    data.Append(dtData.Columns[column].ColumnName.ToString().Replace(",", ";"));
                else
                    data.Append(dtData.Columns[column].ColumnName.ToString().Replace(",", ";") + ',');
            }

            data.Append(Environment.NewLine);//New line after appending columns.

            for (int row = 0; row < dtData.Rows.Count; row++)
            {
                for (int column = 0; column < dtData.Columns.Count; column++)
                {
                    ////Making sure that end of the line, shoould not have comma delimiter.
                    if (column == dtData.Columns.Count - 1)
                        data.Append(dtData.Rows[row][column].ToString().Replace(",", ";"));
                    else
                        data.Append(dtData.Rows[row][column].ToString().Replace(",", ";") + ',');
                }

                //Making sure that end of the file, should not have a new line.
                if (row != dtData.Rows.Count - 1)
                    data.Append(Environment.NewLine);
            }

            try
            {
               
                string contentPath = _hostingEnvironment.ContentRootPath;

                string path = Path.Combine(_hostingEnvironment.ContentRootPath, "ElasticSearchData");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(path, fileName+ ".csv");
                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    sw.WriteLine(data);
                    sw.Close();
                }
                
            }
            catch (Exception ex)
            {
               
            }
        }
