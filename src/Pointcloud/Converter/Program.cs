﻿using Fusee.LASReader;
using Fusee.Math.Core;
using Fusee.Pointcloud.OoCFileReaderWriter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fusee.Examples.PcRendering.Core;
using System.IO;

namespace Fusee.Pointcloud.Converter.LasToOoc
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathToFile = "";
            string pathToFolder = "";
            int maxNoOfPointsInBucket = 0;

            var ptAcc = new PtRenderingAccessor();

            if (args.Length != 3)
            {           
                Console.WriteLine("Enter Path to .las/laz.");
                GetPathToFile(ref pathToFile);

                Console.WriteLine("Enter Path to destination folder.");
                GetPathToFolder(ref pathToFolder);

                Console.WriteLine("Enter max. number of points in Octree bucket.");
                EnterMaxNoOfPts(pathToFile, pathToFolder, ref maxNoOfPointsInBucket);                
            }
            else
            {
                pathToFile = args[0];
                GetPathToFile(ref pathToFile);                

                pathToFolder = args[1];
                GetPathToFolder(ref pathToFolder);

                Int32.TryParse(args[2], out maxNoOfPointsInBucket);
                EnterMaxNoOfPts(pathToFile, pathToFolder, ref maxNoOfPointsInBucket);
            }

            CreateFiles(ptAcc, pathToFile, pathToFolder, maxNoOfPointsInBucket);
        }    
        
        private static void GetPathToFolder(ref string pathToFolder)
        {
           
            pathToFolder = Console.ReadLine();

            try
            {
                var path = Path.GetFullPath(pathToFolder);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine();
                Console.WriteLine("Enter Path to destination folder.");
                pathToFolder = "";
                GetPathToFolder(ref pathToFolder);
            }
        }

        private static void GetPathToFile(ref string pathToFile)
        {  
            pathToFile = Console.ReadLine();
            var extension = Path.GetExtension(pathToFile);

            if(extension == null || (extension!= ".las" && extension != ".laz"))
            {               
                Console.WriteLine("Path is not valid!");
                Console.WriteLine("Enter Path to .las/laz.");
                pathToFile = "";
                GetPathToFile(ref pathToFile);                
            }
            return;
        }

        private static void EnterMaxNoOfPts(string pathToFile, string pathToFolder, ref int mxNo)
        {
            
            var maxNoOfPointsInBucket_str = Console.ReadLine();            

            if (Int32.TryParse(maxNoOfPointsInBucket_str, out var maxNoOfPointsInBucket))
            {
                if (maxNoOfPointsInBucket > 1)
                {                    
                    mxNo = maxNoOfPointsInBucket;
                    return;
                }
                else
                {
                    Console.WriteLine("Number of Points must be > 0");
                    Console.WriteLine("Enter max. number of points in Octree bucket.");
                    EnterMaxNoOfPts(pathToFile, pathToFolder, ref mxNo);                    
                }
            }
            else
            {
                Console.WriteLine("Number of Points must be a number!");
                Console.WriteLine("Enter max. number of points in Octree bucket.");
                EnterMaxNoOfPts(pathToFile, pathToFolder, ref mxNo);                
            }
        }

        private static void CreateFiles(PtRenderingAccessor ptAcc, string pathToFile, string pathToFolder, int maxNoOfPointsInBucket)
        {
            var points = FromLasToList(pathToFile);

            var aabb = new AABBd(points[0].Position, points[0].Position);
            foreach (var pt in points)
            {
                aabb |= pt.Position;
            }

            var watch = new Stopwatch();
            watch.Restart();

            var octree = new PtOctree<LAZPointType>(aabb, ptAcc, points, maxNoOfPointsInBucket);
            Console.WriteLine("Octree creation took: " + watch.ElapsedMilliseconds + "ms.");

            watch.Restart();
            var occFileWriter = new PtOctreeFileWriter<LAZPointType>(pathToFolder);
            occFileWriter.WriteCompleteData(octree, ptAcc);
            Console.WriteLine("Writing files took: " + watch.ElapsedMilliseconds + "ms.");
        }

        internal static List<LAZPointType> FromLasToList(string pathToPc)
        {
            var reader = new LASPointReader(pathToPc);
            var pointCnt = (MetaInfo)reader.MetaInfo;
            var pa = new PtRenderingAccessor();
            var points = new LAZPointType[(int)pointCnt.PointCnt];
            points = points.Select(pt => new LAZPointType()).ToArray();

            for (var i = 0; i < points.Length; i++)
                if (!reader.ReadNextPoint(ref points[i], pa)) break;

            var firstPoint = points[0];

            for (int i = 0; i < points.Length; i++)
            {
                var pt = points[i];
                pt.Position -= firstPoint.Position;
                pt.Position = new double3(pt.Position.x, pt.Position.z, pt.Position.y);

                points[i] = pt;
            }

            reader.Dispose();
            return points.ToList();
        }
    }
}