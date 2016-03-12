using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 *  The PixImage class represents an image, which is a rectangular grid of
 *  color pixels.  Each pixel has red, green, and blue intensities in the range
 *  0...255.  
 */


namespace ImageProcessing
{

    public class PixImage
    {
        private class Pixel
        {
            public short red;
            public short green;
            public short blue;
        }

        /// <summary>
        ///  PixImage's width, height and pixel
        /// </summary>
        private int width;
        private int height;
        private Pixel[,] myImage;

        /// <summary>
        /// PixImage() constructs an empty PixImage with a specified width and height.
        /// Every pixel has red, green, and blue intensities of zero (solid black).
        /// </summary>
        /// <param name="width"> the width of the image. </param>
        /// <param name="height"> the height of the image. </param>
        public PixImage(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.myImage = new Pixel[width, height];

            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    setPixel(x, y, 0, 0, 0);
                }
            }
        }

        /// <summary>
        /// returns the width of the image.
        /// </summary>
        public virtual int Width
        {
            get
            {
                return this.width;
            }
        }

        /// <summary>
        /// returns the height of the image.
        /// </summary>
        public virtual int Height
        {
            get
            {
                return this.height;
            }
        }

        /// <summary>
        /// getRed() returns the red intensity of the pixel at coordinate (x, y).
        /// </summary>
        /// <param name="x"> the x-coordinate of the pixel. </param>
        /// <param name="y"> the y-coordinate of the pixel. </param>
        /// <returns> the red intensity of the pixel at coordinate (x, y). </returns>
        public virtual short getRed(int x, int y)
        {
            return myImage[x, y].red;
        }

        /// <summary>
        /// getGreen() returns the green intensity of the pixel at coordinate (x, y).
        /// </summary>
        /// <param name="x"> the x-coordinate of the pixel. </param>
        /// <param name="y"> the y-coordinate of the pixel. </param>
        /// <returns> the green intensity of the pixel at coordinate (x, y). </returns>
        public virtual short getGreen(int x, int y)
        {
            return myImage[x, y].green;
        }

        /// <summary>
        /// getBlue() returns the blue intensity of the pixel at coordinate (x, y).
        /// </summary>
        /// <param name="x"> the x-coordinate of the pixel. </param>
        /// <param name="y"> the y-coordinate of the pixel. </param>
        /// <returns> the blue intensity of the pixel at coordinate (x, y). </returns>
        public virtual short getBlue(int x, int y)
        {
            return myImage[x,y].blue;
        }
        
        /// <summary>
        /// setPixel() sets the pixel at coordinate (x, y) to specified red, green,
        /// and blue intensities.
        ///   
        /// If any of the three color intensities is NOT in the range 0...255, then
        /// this method does NOT change any of the pixel intensities.
        /// </summary>
        /// <param name="x"> the x-coordinate of the pixel. </param>
        /// <param name="y"> the y-coordinate of the pixel. </param>
        /// <param name="red"> the new red intensity for the pixel at coordinate (x, y). </param>
        /// <param name="green"> the new green intensity for the pixel at coordinate (x, y). </param>
        /// <param name="blue"> the new blue intensity for the pixel at coordinate (x, y). </param>
        public virtual void setPixel(int x, int y, short red, short green, short blue)
        {
            if (myImage[x,y] == null)
            {
                myImage[x, y] = new Pixel();
            }

            if (red < 0 || red > 255 || green < 0 || green > 255 || blue < 0 || blue > 255)
            {
                return;
            }
            
            this.myImage[x, y].red = red;
            this.myImage[x, y].green = green;
            this.myImage[x, y].blue = blue;

        }

        /// <summary>
        /// toString() returns a String representation of this PixImage.
        /// </summary>
        /// <returns> a String representation of this PixImage. </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    sb.Append(myImage[x, y].red.ToString() + ":" + myImage[x, y].green.ToString() + ":" + myImage[x, y].blue.ToString() + "\n");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// boxBlur() returns a blurred version of "this" PixImage.
        /// 
        /// If numIterations == 1, each pixel in the output PixImage is assigned
        /// a value equal to the average of its neighboring pixels in "this" PixImage,
        /// INCLUDING the pixel itself.
        /// 
        /// A pixel not on the image boundary has nine neighbors--the pixel itself and
        /// the eight pixels surrounding it.  A pixel on the boundary has six
        /// neighbors if it is not a corner pixel; only four neighbors if it is
        /// a corner pixel.  The average of the neighbors is the sum of all the
        /// neighbor pixel values (including the pixel itself) divided by the number
        /// of neighbors, with non-integer quotients rounded toward zero (as C# does
        /// naturally when you divide two integers).
        /// 
        /// Each color (red, green, blue) is blurred separately.  The red input should
        /// have NO effect on the green or blue outputs, etc.
        /// 
        /// The parameter numIterations specifies a number of repeated iterations of
        /// box blurring to perform.  If numIterations is zero or negative, "this"
        /// PixImage is returned (not a copy).  If numIterations is positive, the
        /// return value is a newly constructed PixImage.
        /// </summary>
        /// <param name="numIterations"> the number of iterations of box blurring. </param>
        /// <returns> a blurred version of "this" PixImage. </returns>
        public virtual PixImage boxBlur(int numIterations)
        {
            if (numIterations <= 0) return this;

            PixImage prevBlur = this;

            // How much Blur?
            for (int i = 0; i < numIterations; i++)
            {
                PixImage blurImage = new PixImage(width, height);
                // Go through each pixel
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // Get Blur Value
                        Pixel bPixel = getBlurValue(prevBlur, x,y);
                        // Set the new pixel to blurImage
                        blurImage.setPixel(x, y, bPixel.red, bPixel.green, bPixel.blue);
                    }
                }
                prevBlur = blurImage;
            }
            return prevBlur;
        }

        private Pixel getBlurValue(PixImage img, int x, int y)
        {
            Pixel blurP = new Pixel();

            // Find the neighbours, keep track of corner cases and border cases
            int startx = (x - 1 != -1) ? x - 1 : x;
            int starty = (y - 1 != -1) ? y - 1 : y;
            int endx = (x + 1 < width) ? x + 1 : x;
            int endy = (y + 1 < height) ? y + 1 : y;
            int counter = 0, redTotal = 0, greenTotal = 0, blueTotal = 0 ;

            for (int i = starty; i <= endy; i++)
            {
                for (int j = startx; j <= endx; j++)
                {
                    redTotal += img.myImage[j, i].red;
                    greenTotal += img.myImage[j, i].green;
                    blueTotal += img.myImage[j, i].blue;
                    counter++;
                }
            }

            blurP.red = Convert.ToInt16(redTotal / counter);
            blurP.green = Convert.ToInt16(greenTotal / counter);
            blurP.blue = Convert.ToInt16(blueTotal / counter);

            return blurP;
        }


        /// <summary>
        /// mag2gray() maps an energy (squared vector magnitude) in the range
        /// 0...24,969,600 to a grayscale intensity in the range 0...255.  The map
        /// is logarithmic, but shifted so that values of 5,080 and below map to zero.
        /// 
        /// </summary>
        /// <param name="mag"> the energy (squared vector magnitude) of the pixel whose
        /// intensity we want to compute. </param>
        /// <returns> the intensity of the output pixel. </returns>
        private static short mag2gray(long mag)
        {
            short intensity = (short)(30.0 * Math.Log(1.0 + (double)mag) - 256.0);

            // Make sure the returned intensity is in the range 0...255, regardless of
            // the input value.
            if (intensity < 0)
            {
                intensity = 0;
            }
            else if (intensity > 255)
            {
                intensity = 255;
            }
            return intensity;
        }

        /// <summary>
        /// sobelEdges() applies the Sobel operator, identifying edges in "this"
        /// image.  The Sobel operator computes a magnitude that represents how
        /// strong the edge is.  We compute separate gradients for the red, blue, and
        /// green components at each pixel, then sum the squares of the three
        /// gradients at each pixel.  We convert the squared magnitude at each pixel
        /// into a grayscale pixel intensity in the range 0...255 with the logarithmic
        /// mapping encoded in mag2gray().  The output is a grayscale PixImage whose
        /// pixel intensities reflect the strength of the edges.
        ///   
        /// See http://en.wikipedia.org/wiki/Sobel_operator#Formulation for details.
        /// </summary>
        /// <returns> a grayscale PixImage representing the edges of the input image.
        /// Whiter pixels represent stronger edges. </returns>
        public virtual PixImage sobelEdges()
        {
            PixImage img = new PixImage(width, height);
            int[,] xCalMat =
            {
                {1, 0, -1 },
                {2, 0, -2 },
                {1, 0, -1 }
            };

            int[,] yCalMat =
            {
                {1, 2, 1 },
                {0, 0, 0 },
                {-1, -2, -1 }
            };

            long[] gx, gy;
            long[,] energy = new long[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Get the gradient value
                    gx = getGradValue(xCalMat, x, y);
                    gy = getGradValue(yCalMat, x, y);
                    //     energy(x, y) = gx(red)^2 + gy(red)^2 + gx(green)^2 + gy(green)^2 +  gx(blue)^2 + gy(blue)^2. 
                    energy[x, y] = (gx[0] * gx[0]) + (gy[0] * gy[0]) + (gx[1] * gx[1]) + (gy[1] * gy[1]) + (gx[2] * gx[2]) + (gy[2] * gy[2]);
                    img.setPixel(x, y, mag2gray(energy[x, y]), mag2gray(energy[x, y]), mag2gray(energy[x, y]));
                }
            }

            return img;
        }

        private long[] getGradValue(int[,] CalMat, int x, int y)
        {
            long red = 0, green = 0, blue = 0;
            long[] pixel = new long[3];

            // create a long array of 3 elements and put the red, green, blue gx or gy values and return the array
            // Find the neighbours, keep track of corner cases and border cases
            int startx = (x - 1 != -1) ? x - 1 : x;
            int starty = (y - 1 != -1) ? y - 1 : y;
            int endx = (x + 1 < width) ? x + 1 : x;
            int endy = (y + 1 < height) ? y + 1 : y;

            for (int i = startx, l = 0; i <= endx; i++, l++)
            {
                for (int j = starty, m = 0; j <= endy; j++, m++)
                {
                    red = red + (CalMat[l, m] * this.myImage[i, j].red);
                    green = green + (CalMat[l, m] * this.myImage[i, j].green);
                    blue = blue + (CalMat[l, m] * this.myImage[i, j].blue);

                }
            }

            pixel[0] = red;
            pixel[1] = green;
            pixel[2] = blue;

            return pixel;
        }


        /// <summary>
        /// TEST CODE:  
        /// </summary>


        /// <summary>
        /// doTest() checks whether the condition is true and prints the given error
        /// message if it is not.
        /// </summary>
        /// <param name="b"> the condition to check. </param>
        /// <param name="msg"> the error message to print if the condition is false. </param>
        private static void doTest(bool b, string msg)
        {
            if (b)
            {
                Console.WriteLine("Good.");
            }
            else
            {
                Console.Error.WriteLine(msg);
            }
        }

        /// <summary>
        /// array2PixImage() converts a 2D array of grayscale intensities to
        /// a grayscale PixImage.
        /// </summary>
        /// <param name="pixels"> a 2D array of grayscale intensities in the range 0...255. </param>
        /// <returns> a new PixImage whose red, green, and blue values are equal to
        /// the input grayscale intensities. </returns>
        private static PixImage array2PixImage(int[][] pixels)
        {
            int width = pixels.Length;
            int height = pixels[0].Length;
            PixImage image = new PixImage(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    image.setPixel(x, y, (short)pixels[x][y], (short)pixels[x][y], (short)pixels[x][y]);
                }
            }

            return image;
        }

        /// <summary>
        /// equals() checks whether two images are the same, i.e. have the same
        /// dimensions and pixels.
        /// </summary>
        /// <param name="image"> a PixImage to compare with "this" PixImage. </param>
        /// <returns> true if the specified PixImage is identical to "this" PixImage. </returns>
        public virtual bool Equals(PixImage image)
        {
            int width = Width;
            int height = Height;

            if (image == null || width != image.Width || height != image.Height)
            {
                return false;
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!(getRed(x, y) == image.getRed(x, y) && getGreen(x, y) == image.getGreen(x, y) && getBlue(x, y) == image.getBlue(x, y)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// main() runs a series of tests to ensure that the convolutions (box blur
        /// and Sobel) are correct.
        /// </summary>
        public static void Main(string[] args)
        {
            PixImage image1 = array2PixImage(new int[][]
            {
        new int[] {0, 10, 240},
        new int[] {30, 120, 250},
        new int[] {80, 250, 255}
            });
            Console.WriteLine("Testing getWidth/getHeight on a 3x3 image.  " + "Input image:");
            Console.Write(image1);
            doTest(image1.Width == 3 && image1.Height == 3, "Incorrect image width and height.");

            Console.WriteLine("Testing blurring on a 3x3 image.");
            doTest(image1.boxBlur(1).Equals(array2PixImage(new int[][]
            {
        new int[] {40, 108, 155},
        new int[] {81, 137, 187},
        new int[] {120, 164, 218}
            })), "Incorrect box blur (1 rep):\n" + image1.boxBlur(1));
            doTest(image1.boxBlur(2).Equals(array2PixImage(new int[][]
            {
        new int[] {91, 118, 146},
        new int[] {108, 134, 161},
        new int[] {125, 151, 176}
            })), "Incorrect box blur (2 rep):\n" + image1.boxBlur(2));
            doTest(image1.boxBlur(2).Equals(image1.boxBlur(1).boxBlur(1)), "Incorrect box blur (1 rep + 1 rep):\n" + image1.boxBlur(2) + image1.boxBlur(1).boxBlur(1));

            Console.WriteLine("Testing edge detection on a 3x3 image.");
            doTest(image1.sobelEdges().Equals(array2PixImage(new int[][]
            {
        new int[] {104, 189, 180},
        new int[] {160, 193, 157},
        new int[] {166, 178, 96}
            })), "Incorrect Sobel:\n" + image1.sobelEdges());


            PixImage image2 = array2PixImage(new int[][]
            {
        new int[] {0, 100, 100},
        new int[] {0, 0, 100}
            });
            Console.WriteLine("Testing getWidth/getHeight on a 2x3 image.  " + "Input image:");
            Console.Write(image2);
            doTest(image2.Width == 2 && image2.Height == 3, "Incorrect image width and height.");

            Console.WriteLine("Testing blurring on a 2x3 image.");
            doTest(image2.boxBlur(1).Equals(array2PixImage(new int[][]
            {
        new int[] {25, 50, 75},
        new int[] {25, 50, 75}
            })), "Incorrect box blur (1 rep):\n" + image2.boxBlur(1));

            Console.WriteLine("Testing edge detection on a 2x3 image.");
            doTest(image2.sobelEdges().Equals(array2PixImage(new int[][]
            {
        new int[] {122, 143, 74},
        new int[] {74, 143, 122}
            })), "Incorrect Sobel:\n" + image2.sobelEdges());
        }
        
    }
}
