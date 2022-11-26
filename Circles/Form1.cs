using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Circles
{
    public partial class Form1 : Form
    {
        //create list of circles using method createСirсles()
        static Rectangle[] cirсles = createСirсles();
        
        //Method creatingCircles()
        public static Rectangle[] createСirсles()
        {
            // initial coordinates of x and y
            int x = 10;
            int y = 10;

            //create list of 1000 circles
            Rectangle[] cirсles = new Rectangle[1000];
            for(int i =0; i<1000;i++)
            {
                // create a circle
                Rectangle circle = new Rectangle(x, y, 10, 10);

                // add circle to list
                cirсles[i] = circle;

                // define x and y coodinates
                // 50 circles in a row
                x += 15;
                if((i+1)%50 ==0)
                {
                    x = 10;
                    y += 15;
                }
            }
            return cirсles;
        }

        public Form1()
        {
            InitializeComponent();
        }

        // defaine the color of a circle
        Pen red = new Pen(Color.Red);

        // create lock object
        object lockobject = new object();
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // create graphics to 
            Graphics graphics = e.Graphics;
            

            // choose number of threads
            int numberOfThreads = 5;
            //int numberOfThreads = 20;
            //int numberOfThreads = 100;

            // define how many circles each thread will draw
            int sizeOfParts = 1000 / numberOfThreads;

            // define start index for each thread
            int startIndex = 0;

            //define list of Tasks
            var tasks = new List<Task>();

            // start measure time 
            var stopwatch = new Stopwatch();            
            stopwatch.Start();
            
            // start parallel Tasks
            for (int i = 0; i < numberOfThreads; i++)
            {
                tasks.Add(print(startIndex, sizeOfParts));
                startIndex += sizeOfParts;                
            }

            // Wait when tasks finish
            Task.WaitAll(tasks.ToArray());

            // stop measure time
            stopwatch.Stop();

            // print result
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Number of threads: {numberOfThreads}");
            Console.WriteLine($"Time of parallel tasks: {stopwatch.ElapsedMilliseconds}");
            Console.WriteLine();
            Console.WriteLine();


            // method for parallel drawing
            Task print(int startInd, int endInd)
            {
                // define the end index in list of circles to draw 
                endInd=startInd+endInd;
                return Task.Run(async () =>
                {
                    for (int i = startInd; i < endInd; i++)
                    {
                        // delay a Task for 20 msec
                        await Task.Delay(20);
                        lock (lockobject)
                        {
                            // draw circle
                            graphics.DrawEllipse(red, cirсles[i]);                            
                        }
                    }
                    
                });
            }
            
        }        

    }
}
