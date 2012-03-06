﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nasa.MarsRover.Plateau;
using Nasa.MarsRover.Rovers;

namespace Nasa.MarsRover.Report
{
    public class ReportComposer : IReportComposer
    {
        private readonly IDictionary<CardinalDirection, char> cardinalDirectionDictionary;

        public ReportComposer()
        {
            cardinalDirectionDictionary = new Dictionary<CardinalDirection, char>
            {
                 {CardinalDirection.North, 'N'},
                 {CardinalDirection.South, 'S'},
                 {CardinalDirection.East, 'E'},
                 {CardinalDirection.West, 'W'}
            };
        }

        public string Compose(GridPoint aPlateauPoint, CardinalDirection aCardinalDirection)
        {
            var reportItem1 = aPlateauPoint.X;
            var reportItem2 = aPlateauPoint.Y;
            var reportItem3 = cardinalDirectionDictionary[aCardinalDirection];
            var report = new StringBuilder();
            report.AppendFormat("{0} {1} {2}", reportItem1, reportItem2, reportItem3);
            return report.ToString();
        }

        public string CompileReports(IEnumerable<IRover> rovers)
        {
            var reports = composeEachReport(rovers);
            return convertToString(reports);
        }

        private StringBuilder composeEachReport(IEnumerable<IRover> rovers)
        {
            var reports = new StringBuilder();
            foreach (var rover in rovers)
            {
                ensureRoverIsDeployed(rover);
                var report = Compose(rover.Position, rover.CardinalDirection);
                reports.AppendLine(report);
            }
            return reports;
        }

        private static string convertToString(StringBuilder reports)
        {
            return reports.ToString()
                .TrimEnd('\n', '\r');
        }

        private static void ensureRoverIsDeployed(IRover rover)
        {
            if(!rover.IsDeployed())
            {
                throw new ReportException("Cannot create report because one or more rovers are not deployed");
            }
        }
    }
}
