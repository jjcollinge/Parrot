using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using System.Text.RegularExpressions;
using System.IO;

namespace UniverseScheduler
{
    public class EventStreamCSVLoader : IEventStreamFileLoader
    {
        public Queue<UniverseEvent> ReadEventStreamFromFile(string filePath)
        {
            var rows = ReadCSVRowsFromCSVFile(filePath);
            return CreateEventStream(rows);
        }

        /// <summary>
        /// Parse the CSV rows and generate an event stream.
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private Queue<UniverseEvent> CreateEventStream(IList<CsvRow> rows)
        {
            if (rows == null)
                throw new NullReferenceException();

            // Assumes row is in format: [TimeStamp, Id, PropertyA, PropertyB...]
            Queue<UniverseEvent> eventStream = new Queue<UniverseEvent>();

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];

                // Parse time
                var ev = new UniverseEvent();
                var dateTimeStr = Regex.Replace(row[0].Trim(), "[^0-9/:]", " ");
                var originalTime = DateTime.ParseExact(dateTimeStr, "dd/MM/yyyy HH:mm:ss", null);

                ev.OriginalTimeStamp = originalTime;

                ev.ActorId = row[1];

                // Parse payload
                for (int j = 2; j < row.Count; j++)
                {
                    //ev.Payload += row[j] + " ";
                }

                // Remove trailing whitespace
                //ev.Payload.Trim();

                eventStream.Enqueue(ev);
            }

            return eventStream;
        }

        /// <summary>
        /// Load an in memory representation of data in a CSV file.
        /// </summary>
        /// <param name="eventStreamFilePath"></param>
        /// <returns></returns>
        private IList<CsvRow> ReadCSVRowsFromCSVFile(string eventStreamFilePath)
        {
            if (eventStreamFilePath == null)
                throw new NullReferenceException();

            IList<CsvRow> rows = new List<CsvRow>();

            // Read file into memory
            using (var reader = new CsvFileReader(eventStreamFilePath))
            {
                CsvRow row = new CsvRow();
                while (reader.ReadRow(row))
                {
                    rows.Add(row);
                }
            }

            return rows;
        }
    }
}
