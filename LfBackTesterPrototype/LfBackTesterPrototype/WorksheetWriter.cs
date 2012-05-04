// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorksheetWriter.cs" company="The Logans Ferry Software Co.">
//   Copyright 2012, The Logans Ferry Software Co.
// </copyright>
// <license>  
//   This file is part of LfBackTesterPrototype.
//   
//   LfBackTesterPrototype is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   LfBackTesterPrototype is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   LfBackTesterPrototype. If not, see http://www.gnu.org/licenses/.
// </license>
// <author>Aaron Morris</author>
// <last_updated>04/29/2012 9:40 AM</last_updated>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.BackTesterPrototype
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Writes the results of back-testing to the file system.
    /// </summary>
    public class WorksheetWriter
    {
        /// <summary>
        /// Writes the worksheet to CSV.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="worksheet">The worksheet.</param>
        public void WriteWorksheetToCsv(string filename, IEnumerable<WorksheetEntry> worksheet)
        {
            const char Delimiter = ',';

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            var csvFile = File.CreateText(filename);

            var header =
                "Date" + Delimiter
                + "Open" + Delimiter
                + "High" + Delimiter
                + "Stop" + Delimiter
                + "Low" + Delimiter
                + "Close" + Delimiter
                + "Volume" + Delimiter
                + "Price Direction" + Delimiter
                + "ADX" + Delimiter
                + "ADX Direction" + Delimiter
                + "52-Week %-age" + Delimiter
                + "Volatility Range" + Delimiter
                + "Short Term High" + Delimiter
                + "Short Term Low" + Delimiter
                + "Intermediate High" + Delimiter
                + "Intermediate Low" + Delimiter
                + "Long Term High" + Delimiter
                + "Long Term Low" + Delimiter
                + "Balance" + Delimiter
                + "Position Size" + Delimiter
                + "52-Week High" + Delimiter
                + "52-Week Low" + Delimiter
                + "OBV" + Delimiter
                + "OBV Strength" + Delimiter
                + "EMA (5)" + Delimiter
                + "EMA (20)";

            csvFile.WriteLine(header);

            foreach (var entry in worksheet)
            {
                var csvLine =
                    entry.Date + Delimiter
                    + entry.OpenPrice + Delimiter
                    + entry.HighPrice + Delimiter
                    + entry.CurrentStop + Delimiter
                    + entry.LowPrice + Delimiter
                    + entry.ClosePrice + Delimiter
                    + entry.Volume + Delimiter
                    + entry.PriceDirection + Delimiter
                    + entry.Adx + Delimiter
                    + entry.AdxDirection + Delimiter
                    + entry.FiftyTwoWeekPercentage + Delimiter
                    + entry.VolatilityRange + Delimiter
                    + entry.ShortTermHigh + Delimiter
                    + entry.ShortTermLow + Delimiter
                    + entry.IntermediateHigh + Delimiter
                    + entry.IntermediateLow + Delimiter
                    + entry.LongTermHigh + Delimiter
                    + entry.LongTermLow + Delimiter
                    + entry.CurrentBalance + Delimiter
                    + entry.CurrentPositionSize + Delimiter
                    + entry.FiftyTwoWeekHigh + Delimiter
                    + entry.FiftyTwoWeekLow + Delimiter
                    + entry.OnBalanceVolume + Delimiter
                    + entry.OnBalanceVolumeStrength + Delimiter
                    + entry.Ema5 + Delimiter
                    + entry.Ema20;

                csvFile.WriteLine(csvLine);
            }

            csvFile.Close();
        }
    }
}
