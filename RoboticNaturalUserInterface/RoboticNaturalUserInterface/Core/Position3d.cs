﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.Core
{

    public enum Dimension { X, Y, Z }

    /**
     * <summary>
     * 3 Dimensional position
     * 
     * This class offers getters and setters for rectangular, cylindrical, and spherical coordinate systems.
     * 
     * </summary>
     * <remarks>
     * Author: Jon Eisen (yanatan16@gmail.com)
     * </remarks>
     */
    public class Position3d
    {
        /**
         * <summary>
         * Rectangular horizontal distance from center
         * </summary>
         * <remarks>
         * Range: All Reals
         * </remarks>
         */
        public double x { get; set; }

        /**
         * <summary>
         * Rectangular vertical distance from center
         * </summary>
         * <remarks>
         * Range: All Reals
         * </remarks>
         */
        public double y { get; set; }

        /**
         * <summary>
         * Rectangular and Cylindrical depth from camera
         * </summary>
         * <remarks>
         * Range: Positive Reals
         * </remarks>
         */
        public double z { get; set; }

#if false
        /**
         * <summary>
         * Spherical angle from x=0, y=0 depth line in view of camera (no depth)
         * </summary>
         * <remarks>
         * Range: Zero to Pi
         * </remarks>
         */
        public double theta
        {
            get
            {
                theta = Math.Atan2(z, Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2))) % Math.PI;
                if (theta < 0)
                {
                    theta += Math.PI;
                }
                return theta;
            }

            set
            {
                theta = value;
                x = r * Math.Sin(theta) * Math.Cos(phi);
                y = r * Math.Sin(theta) * Math.Cos(phi);
                z = r * Math.Cos(theta);
            }
        }

        /**
         * <summary>
         * Spherical Magnitude of 3-demensional distant
         * </summary>
         * <remarks>
         * Range: Non-negative Reals
         * </remarks>
         */
        public double r
        {
            get
            {
                r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(z, 2) + Math.Pow(z, 2));
                return r;
            }

            set
            {
                r = value;
                x = r * Math.Sin(theta) * Math.Cos(phi);
                y = r * Math.Sin(theta) * Math.Cos(phi);
                z = r * Math.Cos(theta);
                rho = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            }
        }

        /**
         * <summary>
         * Spherical and Cylindrical angle from z=0, y=0 line in horizontal plane
         * </summary>
         * <remarks>
         * Range: 0 to 2*Pi
         * </remarks>
         */
        public double phi
        {
            get
            {
                phi = Math.Atan2(y, x) % (2 * Math.PI);
                if (phi < 0)
                {
                    phi += 2 * Math.PI;
                }
                return phi;
            }

            set
            {
                phi = value;
                x = rho * Math.Cos(phi);
                y = rho * Math.Sin(phi);
            }
        } 

        /**
         * <summary>
         * Cylindrical z=0 plane range
         * </summary>
         * <remarks>
         * Range: Non-negative reals
         * </remarks>
         */
        public double rho
        {
            get
            {
                rho = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                return rho;
            }
            set
            {
                rho = value;
                x = rho * Math.Cos(phi);
                y = rho * Math.Sin(phi);
            }
        }

        /**
         * <summary>
         * Constructor with spherical coordinates
         * </summary>
         */
        public Position3d(double r, double phi, double theta, bool unused)
        {
            this.r = r;
            this.phi = phi;
            this.theta = theta;
        }
#endif

        /**
         * <summary>
         * Default Constructor
         * </summary>
         */
        public Position3d() : this(0,0,0)
        {
        }

        public Position3d(Position3d v)
            : this(v.x, v.y, v.z)
        { }

        /**
         * <summary>
         * Constructor with rectangular coordinates
         * </summary>
         */
        public Position3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /**
         * <summary>Magnitude (2-norm) of this vector</summary>
         * <returns>Magnitude of this vector</returns>
         */
        public double Magnitude()
        {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
        }

        /**
         * <summary>Dot product with b</summary>
         * <param name="b">Second vector to dot product</param>
         * <returns>Dot product with b</returns>
         */
        public double Dot(Position3d b)
        {
            return x * b.x + y * b.y + z * b.z;
        }

        public Position3d Cross(Position3d b)
        {
            return new Position3d(y * b.z - z * b.y, z * b.x - x * b.z, x * b.y - y * b.x);
        }

        public double angle(Position3d that)
        {
            return Math.Acos(this.Dot(that) / (this.Magnitude() * that.Magnitude()));
        }

        /**
         * <summary>Subtract two Position3d's</summary>
         * <param name="a">Positive element in sum</param>
         * <param name="b">Negative element in sum</param>
         * <returns>A Position3d object that is the sum of a and -b.</returns>
         */
        public static Position3d operator -(Position3d a, Position3d b)
        {
            return a + (-b);
        }

        public static Position3d operator -(Position3d that)
        {
            return new Position3d(-that.x, -that.y, -that.z);
        }

        public static Position3d operator -(double a, Position3d x)
        {
            return (-x) + a;
        }
        public static Position3d operator -(Position3d x, double a)
        {
            return x + (-a);
        }

        public static Position3d operator +(Position3d x, double a)
        {
            return new Position3d(x.x + a, x.y + a, x.z + a);
        }

        public static Position3d operator +(Position3d x, Position3d y)
        {
            return new Position3d(x.x + y.x, x.y + y.y, x.z + y.z);
        }
        public static Position3d operator *(Position3d x, double a)
        {
            return new Position3d(a * x.x, a * x.y, a * x.z);
        }

        public static Position3d operator *(double a, Position3d x)
        {
            return x * a;
        }

        public static Position3d operator /(Position3d x, double a)
        {
            return x * (1/a);
        }

        public override string ToString()
        {
            return "[" + x + "," + y + "," + z + "]";
        }
    }
}
