using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engineering
{
    public class NCProperties
    {
        // beam dimension variables
        public static double bft, tft, D, tw, bfb, tfb;
        // beam property variables
        public static double Atf, Aweb, Abf, Ytf, Yweb, Ybf, Itf, Iweb, Ibf, area, I, CG, NA, Stop, Sbot, PNA, PCGtop, PCGbot, Z;
        // beam property calculations
        public static double PartArea(double width, double height)
        {
            area = width * height;
            return area;
        }
        public static double PartCG(double first, double second, double last)
        {
            CG = first + second + last / 2;
            return CG;
        }
        public static double BeamArea(double bft, double tft, double D, double tw, double bfb, double tfb)
        {
            Atf = NCProperties.PartArea(bft, tft);
            Aweb = NCProperties.PartArea(D, tw);
            Abf = NCProperties.PartArea(bfb, tfb);
            area = Atf + Aweb + Abf;
            return area;
        }
        public static double NeutralAxis(double bft, double tft, double D, double tw, double bfb, double tfb)
        {
            Atf = NCProperties.PartArea(bft, tft);
            Aweb = NCProperties.PartArea(D, tw);
            Abf = NCProperties.PartArea(bfb, tfb);
            Ytf = NCProperties.PartCG(tfb, D, tft);
            Yweb = NCProperties.PartCG(tfb, 0, D);
            Ybf = NCProperties.PartCG(0, 0, tfb);
            NA = (Atf * Ytf + Aweb * Yweb + Abf * Ybf) / (Atf + Aweb + Abf);
            return NA;
        }
        public static double PartMomOfInert(double width, double height)
        {
            I = width * Math.Pow(height, 3) / 12;
            return I;
        }
        public static double MomentOfIneria(double bft, double tft, double D, double tw, double bfb, double tfb)
        {
            Atf = NCProperties.PartArea(bft, tft);
            Aweb = NCProperties.PartArea(D, tw);
            Abf = NCProperties.PartArea(bfb, tfb);
            Ytf = NCProperties.PartCG(tfb, D, tft);
            Yweb = NCProperties.PartCG(tfb, 0, D);
            Ybf = NCProperties.PartCG(0, 0, tfb);
            NA = NCProperties.NeutralAxis(bft, tft, D, tw, bfb, tfb);
            Itf = NCProperties.PartMomOfInert(bft, tft);
            Iweb = NCProperties.PartMomOfInert(tw, D);
            Ibf = NCProperties.PartMomOfInert(bfb, tfb);
            I = Itf + Atf * Math.Pow(Ytf - NA, 2) + Iweb + Aweb * Math.Pow(Yweb - NA, 2) + Ibf + Abf * Math.Pow(Ybf - NA, 2);
            return I;
        }
        public static double ElastSectModTop(double bft, double tft, double D, double tw, double bfb, double tfb)
        {
            Ytf = NCProperties.PartCG(tfb, D, tft);
            NA = NCProperties.NeutralAxis(bft, tft, D, tw, bfb, tfb);
            I = NCProperties.MomentOfIneria(bft, tft, D, tw, bfb, tfb);
            Stop = I / (Ytf - NA + tft / 2);
            return Stop;
        }
        public static double ElastSectModBot(double bft, double tft, double D, double tw, double bfb, double tfb)
        {
            Ybf = NCProperties.PartCG(0, 0, tfb);
            NA = NCProperties.NeutralAxis(bft, tft, D, tw, bfb, tfb);
            I = NCProperties.MomentOfIneria(bft, tft, D, tw, bfb, tfb);
            Sbot = I / (NA - Ybf + tfb / 2);
            return Sbot;
        }
        public static double PlastNeutralAxis(double bft, double tft, double D, double tw, double bfb, double tfb)
        {
            PNA = tfb + (D + (bft * tft - bfb * tfb) / tw) / 2;
            return PNA;
        }
        public static double PNAtoTopCG(double bft, double tft, double D, double tw, double bfb, double tfb)
        {
            area = NCProperties.BeamArea(bft, tft, D, tw, bfb, tfb);
            PNA = NCProperties.PlastNeutralAxis(bft, tft, D, tw, bfb, tfb);
            PCGtop = (bft * tft * (tft / 2 + D + tfb - PNA) + tw * Math.Pow(D - PNA + tfb, 2) / 2) * 2 / area;
            return PCGtop;
        }
        public static double PNAtoBotCG(double bft, double tft, double D, double tw, double bfb, double tfb)
        {
            area = NCProperties.BeamArea(bft, tft, D, tw, bfb, tfb);
            PNA = NCProperties.PlastNeutralAxis(bft, tft, D, tw, bfb, tfb);
            PCGbot = (bfb * tfb * (PNA - tfb / 2) + tw * Math.Pow(PNA - tfb, 2) / 2) * 2 / area;
            return PCGbot;
        }
        public static double PlastSectMod(double bft, double tft, double D, double tw, double bfb, double tfb)
        {
            area = NCProperties.BeamArea(bft, tft, D, tw, bfb, tfb);
            PNA = NCProperties.PlastNeutralAxis(bft, tft, D, tw, bfb, tfb);
            PCGtop = NCProperties.PNAtoTopCG(bft, tft, D, tw, bfb, tfb);
            PCGbot = NCProperties.PNAtoBotCG(bft, tft, D, tw, bfb, tfb);
            Z = area / 2 * (PCGtop + PCGbot);
            return Z;
        }
    }
    public class CompProperties
    {
        // beam dimension variables
        public static double Fy,fs,fc,bft, tft, D, tw, bfb, tfb,bslab,tslab,n,Asteel1,Asteel2,distSteel1,distSteel2,haunchDepth,haunchWidth, PFt,PFb, Dp,Dt, Mp;
        // beam property variables
        public static double Atf, Aweb, Abf,Aslab,Ahaunch, Ytf, Yweb, Ybf,Yslab,Ysteel1,Ysteel2, Yhaunch, Itf, Iweb, Ibf,Islab, Ihaunch, area, I, CG, NA, Stop, Sbot, PNA, PCGtop, PCGbot, Z;
        // beam property calculations
        public static double PartArea(double width, double height,double n)
        {
            area = width * height*n;
            return area;
        }
        public static double PartCG(double first, double second,double third,double fourth, double last)
        {
            CG = first + second + third + fourth + last / 2;
            return CG;
        }
        public static double SteelCG(double tfb, double D, double tft, double haunchDepth, double slab, double dist)
        {
            double SteelCG = tfb + D + tft + haunchDepth + slab - dist;
            return SteelCG;
        }
        public static double BeamArea(double bft, double tft, double D, double tw, double bfb, double tfb,double bslab, double tslab,double n, double Asteel1, double Asteel2,double haunchDepth, double haunchWidth)
        {
            Atf = CompProperties.PartArea(bft, tft,1);
            Aweb = CompProperties.PartArea(D, tw, 1);
            Abf = CompProperties.PartArea(bfb, tfb, 1);
            Aslab = CompProperties.PartArea(bslab, tslab, 1 / n);
            Ahaunch = CompProperties.PartArea(haunchDepth, haunchWidth, 1 / n);
            area = Atf + Aweb + Abf + Aslab + Ahaunch + Asteel1 + Asteel2;
            return area;
        }
        public static double NeutralAxis(double bft, double tft, double D, double tw, double bfb, double tfb, double bslab, double tslab, double n, double Asteel1, double Asteel2, double distSteel1, double distSteel2, double haunchDepth, double haunchWidth)
        {
            Atf = CompProperties.PartArea(bft, tft, 1);
            Aweb = CompProperties.PartArea(D, tw, 1);
            Abf = CompProperties.PartArea(bfb, tfb, 1);
            Ahaunch = CompProperties.PartArea(haunchWidth , haunchDepth, 1 / n);
            Aslab = CompProperties.PartArea(tslab, bslab, 1 / n);
            Ytf = CompProperties.PartCG(tfb, D,0,0, tft);
            Yweb = CompProperties.PartCG(tfb, 0,0,0, D);
            Ybf = CompProperties.PartCG(0, 0,0,0, tfb);
            Yhaunch = CompProperties.PartCG(tfb, D, tft, 0, haunchDepth);
            Yslab=CompProperties.PartCG(tfb,D,tft,haunchDepth,tslab);
            Ysteel1 = CompProperties.SteelCG(bft, D, tft, haunchDepth, tslab, distSteel1);
            Ysteel2 = CompProperties.SteelCG(bft, D, tft, haunchDepth, tslab, distSteel2);
            NA = (Atf * Ytf + Aweb * Yweb + Abf * Ybf + Ahaunch * Yhaunch + Aslab * Yslab + Asteel1 * Ysteel1 + Asteel2 * Ysteel2) / (Atf + Aweb + Abf + Ahaunch + Aslab + Asteel1 + Asteel2);
            return NA;
        }
        public static double PartMomOfInert(double width, double height,double n)
        {
            I = width / n * Math.Pow(height, 3) / 12;
            return I;
        }
        public static double MomentOfIneria(double bft, double tft, double D, double tw, double bfb, double tfb, double bslab, double tslab, double n, double Asteel1, double Asteel2, double distSteel1, double distSteel2, double haunchDepth, double haunchWidth)
        {
            Atf = CompProperties.PartArea(bft, tft, 1);
            Aweb = CompProperties.PartArea(D, tw, 1);
            Abf = CompProperties.PartArea(bfb, tfb, 1);
            Ahaunch = CompProperties.PartArea(haunchWidth , haunchDepth, 1 / n);
            Aslab = CompProperties.PartArea(tslab, bslab, 1 / n);
            Ytf = CompProperties.PartCG(tfb, D, 0, 0, tft);
            Yweb = CompProperties.PartCG(tfb, 0, 0, 0, D);
            Ybf = CompProperties.PartCG(0, 0, 0, 0, tfb);
            Yhaunch = CompProperties.PartCG(tfb, D, tft, 0, haunchDepth);
            Yslab = CompProperties.PartCG(tfb, D, tft, haunchDepth, tslab);
            Ysteel1 = CompProperties.SteelCG(bft, D, tft, haunchDepth, tslab, distSteel1);
            Ysteel2 = CompProperties.SteelCG(bft, D, tft, haunchDepth, tslab, distSteel2);
            NA = CompProperties.NeutralAxis(bft, tft, D, tw, bfb, tfb,bslab,tslab,n,Asteel1,Asteel2,distSteel1,distSteel2,haunchDepth,haunchWidth);
            Itf = CompProperties.PartMomOfInert(bft, tft,1);
            Iweb = CompProperties.PartMomOfInert(tw, D,1);
            Ibf = CompProperties.PartMomOfInert(bfb, tfb,1);
            Ihaunch = CompProperties.PartMomOfInert(haunchWidth, haunchWidth, n);
            Islab = CompProperties.PartMomOfInert(bslab, tslab, n);
            I = Itf + Atf * Math.Pow(Ytf - NA, 2) + Iweb + Aweb * Math.Pow(Yweb - NA, 2) + Ibf + Abf * Math.Pow(Ybf - NA, 2) + Ihaunch + Ahaunch * Math.Pow(Yhaunch - NA, 2) + Islab + Aslab * Math.Pow(Yslab - NA, 2) + Asteel1 * Math.Pow(Ysteel1 - NA, 2) + Asteel2 * Math.Pow(Ysteel2 - NA, 2);
            return I;
        }
        public static double ElastSectModTop(double bft, double tft, double D, double tw, double bfb, double tfb, double bslab, double tslab, double n, double Asteel1, double Asteel2, double distSteel1, double distSteel2, double haunchDepth, double haunchWidth)
        {
            Ytf = CompProperties.PartCG(tfb, D,0,0, tft);
            NA = CompProperties.NeutralAxis(bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
            I = CompProperties.MomentOfIneria(bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
            Stop = I / (Ytf - NA + tft / 2);
            return Stop;
        }
        public static double ElastSectModBot(double bft, double tft, double D, double tw, double bfb, double tfb, double bslab, double tslab, double n, double Asteel1, double Asteel2, double distSteel1, double distSteel2, double haunchDepth, double haunchWidth)
        {
            Ybf = NCProperties.PartCG(0, 0, tfb);
            NA = CompProperties.NeutralAxis(bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
            I = CompProperties.MomentOfIneria(bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
            Sbot = I / (NA - Ybf + tfb / 2);
            return Sbot;
        }
        public static double PlastNeutralAxis(double Fy, double fc, double fs,double bft, double tft, double D, double tw, double bfb, double tfb, double bslab, double tslab, double n, double Asteel1, double Asteel2, double distSteel1, double distSteel2, double haunchDepth, double haunchWidth)
        {
            Atf = CompProperties.PartArea(bft, tft, 1);
            Aweb = CompProperties.PartArea(D, tw, 1);
            Abf = CompProperties.PartArea(bfb, tfb, 1);
            Ahaunch = CompProperties.PartArea(haunchWidth , haunchDepth, 0.85 );
            Aslab = CompProperties.PartArea(tslab, bslab, 0.85 );
            // dist to PNA from top of slab if located in slab
            double a1 = ((Atf + Aweb + Abf) * Fy - (Asteel1 + Asteel2) * fs) / (bslab * 0.85 * fc);
            // dist to PNA from top of haunch if located in haunch
            double a2 = ((Atf + Aweb + Abf) * Fy - (Asteel1 + Asteel2) * fs - Aslab * fc) / (haunchWidth * 0.85 * fc);
            // dist to PNA from top of top flange if located in top flange
            double a3 = ((Atf + Aweb + Abf) * Fy - (Asteel1 + Asteel2) * fs - (Aslab + Ahaunch) * fc) / (2 * bft * Fy);
            // dist to PNA from top of web if located in web
            double a4 = ((Aweb + Abf - Atf) * Fy - (Asteel1 + Asteel2) * fs - (Aslab + Ahaunch) * fc) / (2 * tw * Fy);

            if (Aslab * fc + (Asteel1 + Asteel2) * fs > Ahaunch*fc+(Atf + Aweb + Abf) * Fy)
            {
                PNA = tfb + D + tft + haunchDepth + tslab - a1;
            }
            else if ((Aslab + Ahaunch) * fc + (Asteel1 + Asteel2) * fs > (Atf + Aweb + Abf) * Fy)
            {
                PNA = tfb + D + tft + haunchDepth - a2;
            }
            else if ((Aslab + Ahaunch) * fc + (Asteel1 + Asteel2) * fs + Atf * Fy > (Aweb + Abf) * Fy)
            {
                PNA = tfb + D + tft - a3;
            }
            else
            {
                PNA = tfb + D - a4;
            }
            return PNA;
        }
        public static double PlastForceTop(double Fy, double fc, double fs, double bft, double tft, double D, double tw, double bfb, double tfb, double bslab, double tslab, double n, double Asteel1, double Asteel2, double distSteel1, double distSteel2, double haunchDepth, double haunchWidth)
        {
            Atf = CompProperties.PartArea(bft, tft, 1);
            Aweb = CompProperties.PartArea(D, tw, 1);
            Abf = CompProperties.PartArea(bfb, tfb, 1);
            Ahaunch = CompProperties.PartArea(haunchWidth, haunchDepth, 0.85);
            Aslab = CompProperties.PartArea(tslab, bslab, 0.85);
            // dist to PNA from top of slab if located in slab
            double a1 = ((Atf + Aweb + Abf) * Fy - (Asteel1 + Asteel2) * fs) / (bslab * 0.85 * fc);
            // dist to PNA from top of haunch if located in haunch
            double a2 = ((Atf + Aweb + Abf) * Fy - (Asteel1 + Asteel2) * fs - Aslab * fc) / (haunchWidth * 0.85 * fc);
            // dist to PNA from top of top flange if located in top flange
            double a3 = ((Atf + Aweb + Abf) * Fy - (Asteel1 + Asteel2) * fs - (Aslab + Ahaunch) * fc) / (2 * bft * Fy);
            // dist to PNA from top of web if located in web
            double a4 = ((Aweb + Abf - Atf) * Fy - (Asteel1 + Asteel2) * fs - (Aslab + Ahaunch) * fc) / (2 * tw * Fy);
            PNA = CompProperties.PlastNeutralAxis(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);

            if (PNA>tfb+D+tft+haunchDepth)
            {
                PFt = bslab * a1 * 0.85 * fc + (Asteel1 + Asteel2) * fs;
            }
            else if (PNA>tfb+D+tft)
            {
                PFt = (Aslab + haunchWidth * a2 * 0.85) * fc + (Asteel1 + Asteel2) * fs;
            }
            else if (PNA>tfb+D)
            {
                PFt = (Aslab + Ahaunch) * fc + bft * a3*Fy + (Asteel1 + Asteel2) * fs;
            }
            else
            {
                PFt = (Aslab + Ahaunch) * fc + Atf * Fy + tw * a4 * Fy + (Asteel1 + Asteel2) * fs;
            }
            return PFt;
        }
        public static double PlastForceBot(double Fy, double fc, double fs, double bft, double tft, double D, double tw, double bfb, double tfb, double bslab, double tslab, double n, double Asteel1, double Asteel2, double distSteel1, double distSteel2, double haunchDepth, double haunchWidth)
        {
            Atf = CompProperties.PartArea(bft, tft, 1);
            Aweb = CompProperties.PartArea(D, tw, 1);
            Abf = CompProperties.PartArea(bfb, tfb, 1);
            Ahaunch = CompProperties.PartArea(haunchWidth, haunchDepth, 0.85);
            Aslab = CompProperties.PartArea(tslab, bslab, 0.85);
            // dist to PNA from top of slab if located in slab
            double a1 = ((Atf + Aweb + Abf) * Fy - (Asteel1 + Asteel2) * fs) / (bslab * 0.85 * fc);
            // dist to PNA from top of haunch if located in haunch
            double a2 = ((Atf + Aweb + Abf) * Fy - (Asteel1 + Asteel2) * fs - Aslab * fc) / (haunchWidth * 0.85 * fc);
            // dist to PNA from top of top flange if located in top flange
            double a3 = ((Atf + Aweb + Abf) * Fy - (Asteel1 + Asteel2) * fs - (Aslab + Ahaunch) * fc) / (2 * bft * Fy);
            // dist to PNA from top of web if located in web
            double a4 = ((Aweb + Abf - Atf) * Fy - (Asteel1 + Asteel2) * fs - (Aslab + Ahaunch) * fc) / (2 * tw * Fy);
            PNA = CompProperties.PlastNeutralAxis(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);

            if (PNA > tfb + D + tft)
            {
                PFb = (Abf+Aweb+Atf)*Fy;
            }
            else if (PNA > tfb + D)
            {
                PFb = (Abf+Aweb+(tft-a3)*bft)*Fy;
            }
            else
            {
                PFb = (Abf+tw*(D-a4))*Fy;
            }
            return PFb;
        }
        public static double PNAtoTopCG(double Fy, double fc, double fs, double bft, double tft, double D, double tw, double bfb, double tfb, double bslab, double tslab, double n, double Asteel1, double Asteel2, double distSteel1, double distSteel2, double haunchDepth, double haunchWidth)
        {
            Atf = CompProperties.PartArea(bft, tft,1);
            Aweb = CompProperties.PartArea(D, tw,1);
            Abf = CompProperties.PartArea(bfb, tfb,1);
            Ahaunch = CompProperties.PartArea(haunchWidth, haunchDepth, 0.85);
            Aslab = CompProperties.PartArea(tslab, bslab, 0.85);
            area = CompProperties.BeamArea(bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, haunchDepth, haunchWidth);
            PNA = CompProperties.PlastNeutralAxis(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
            // dist to PNA from top of slab if located in slab
            double a1 = ((Atf + Aweb + Abf) * Fy - (Asteel1 + Asteel2) * fs) / (bslab * 0.85 * fc);
            // dist to PNA from top of haunch if located in haunch
            double a2 = ((Atf + Aweb + Abf) * Fy - (Asteel1 + Asteel2) * fs - Aslab * fc) / (haunchWidth * 0.85 * fc);
            // dist to PNA from top of top flange if located in top flange
            double a3 = ((Atf + Aweb + Abf) * Fy - (Asteel1 + Asteel2) * fs - (Aslab + Ahaunch) * fc) / (2 * bft * Fy);
            // dist to PNA from top of web if located in web
            double a4 = ((Aweb + Abf - Atf) * Fy - (Asteel1 + Asteel2) * fs - (Aslab + Ahaunch) * fc) / (2 * tw * Fy);


            if (PNA > tfb + D + tft + haunchDepth)
            {
                PCGtop = a1 / 2;
            }
            else if (PNA > tfb + D + tft)
            {
                PCGtop = (Asteel1 * fs * (tslab - distSteel1 + a2) + Asteel2 * fs * (tslab - distSteel2 + a2) + Aslab * fc * (tslab / 2 + a2) + haunchWidth * fc * Math.Pow(a2, 2)) / ((Asteel1 + Asteel2) * fs + Aslab * fc + haunchWidth * fc * a2);
            }
            else if (PNA > tfb + D)
            {
                PCGtop = (Asteel1 * fs * (tslab - distSteel1 + haunchDepth + a3) + Asteel2 * fs * (tslab - distSteel2 + haunchDepth + a3) + Aslab * fc * (tslab / 2 + haunchDepth + a3) + Ahaunch * fc * (haunchDepth / 2 + a3) + bft * Fy * Math.Pow(a3, 2) / 2) / ((Asteel1 + Asteel2) * fs + Aslab * fc + Ahaunch * fc + bft * Fy * a3);
            }
            else 
            {
                PCGtop = (Asteel1 * fs * (tslab - distSteel1 + haunchDepth + tft + a4) + Asteel2 * fs * (tslab - distSteel2 + haunchDepth + tft + a4) + Aslab * fc * (tslab / 2 + haunchDepth + tft + a4) + Ahaunch * fc * (haunchDepth / 2 + tft + a4) + Atf * Fy * (tft / 2 + a4) + tw * Fy * Math.Pow(a4, 2) / 2) / ((Asteel1 + Asteel2) * fs + Aslab * fc + Ahaunch * fc + Atf * Fy + tw * Fy * a4);
            }
            return PCGtop;
        }
        public static double PNAtoBotCG(double Fy, double fc, double fs, double bft, double tft, double D, double tw, double bfb, double tfb, double bslab, double tslab, double n, double Asteel1, double Asteel2, double distSteel1, double distSteel2, double haunchDepth, double haunchWidth)
        {
            Atf = CompProperties.PartArea(bft, tft, 1);
            Aweb = CompProperties.PartArea(D, tw, 1);
            Abf = CompProperties.PartArea(bfb, tfb, 1);
            Ahaunch = CompProperties.PartArea(haunchWidth, haunchDepth, 0.85);
            Aslab = CompProperties.PartArea(tslab, bslab, 0.85);
            area = CompProperties.BeamArea(bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, haunchDepth, haunchWidth);
            PNA = CompProperties.PlastNeutralAxis(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
            // dist to PNA from top of slab if located in slab
            double a1 = ((Atf + Aweb + Abf) * Fy - (Asteel1 + Asteel2) * fs) / (bslab * 0.85 * fc);
            // dist to PNA from top of haunch if located in haunch
            double a2 = ((Atf + Aweb + Abf) * Fy - (Asteel1 + Asteel2) * fs - Aslab * fc) / (haunchWidth * 0.85 * fc);
            // dist to PNA from top of top flange if located in top flange
            double a3 = ((Atf + Aweb + Abf) * Fy - (Asteel1 + Asteel2) * fs - (Aslab + Ahaunch) * fc) / (2 * bft * Fy);
            // dist to PNA from top of web if located in web
            double a4 = ((Aweb + Abf - Atf) * Fy - (Asteel1 + Asteel2) * fs - (Aslab + Ahaunch) * fc) / (2 * tw * Fy);
            PNA = CompProperties.PlastNeutralAxis(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
            if (PNA > tfb + D + tft + haunchDepth)
            {
                PCGbot = (Abf*(tfb/2+D+tft+haunchDepth+tslab-a1)+Aweb*(D/2+tft+haunchDepth+tslab-a1)+Atf*(tft/2+haunchDepth+tslab-a1))/(Abf+Aweb+Atf);
            }
            else if (PNA > tfb + D + tft)
            {
                PCGbot = (Abf * (tfb / 2 + D + tft + haunchDepth - a2) + Aweb * (D / 2 + tft + haunchDepth - a2) + Atf * (tft / 2 + haunchDepth - a2)) / (Abf + Aweb + Atf);
            }
            else if (PNA > tfb + D)
            {
                PCGbot = (Abf * (tfb / 2 + D + tft - a3) + Aweb * (D / 2 + tft - a3) + bft * Math.Pow(tft - a3, 2) / 2) / (Abf + Aweb + bft * (tft - a3));
            }
            else
            {
                PCGbot = (Abf * (tfb / 2 + D - a4) + tw * Math.Pow(D - a4, 2) / 2) / (Abf + tw * (D - a4));
            }
            return PCGbot;
        }
        public static double PlastMoment(double Fy, double fc, double fs, double bft, double tft, double D, double tw, double bfb, double tfb, double bslab, double tslab, double n, double Asteel1, double Asteel2, double distSteel1, double distSteel2, double haunchDepth, double haunchWidth)
        {
            PNA = CompProperties.PlastNeutralAxis(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
            Dt = tfb + D + tft + haunchDepth + tslab;
            Dp = Dt - PNA;
            PCGtop = CompProperties.PNAtoTopCG(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
            PCGbot = CompProperties.PNAtoBotCG(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
            PFt = CompProperties.PlastForceTop(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
            PFb = CompProperties.PlastForceBot(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
            if (Dp <= 0.1 * Dt)
            {
                Mp = (PFt * PCGtop + PFb * PCGbot)/12;
            }
            else
            {
                Mp = (PFt * PCGtop + PFb * PCGbot)*(1.07-0.7*Dp/Dt)/12;
            }
            return Mp;
        }
    }
    public class Beams
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter Number of Beams : ");
            int NoBeams = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter Beam Yield Stress, Fy (ksi) : ");
            double Fy = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter Slab Compressive Stress, f'c (ksi) : ");
            double fc = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter Rebar Yield Stress, fs (ksi) : ");
            double fs = Convert.ToDouble(Console.ReadLine()); 
            Console.WriteLine("Enter Beam modular ratio, n : ");
            double n = Convert.ToDouble(Console.ReadLine()); 
            double[,] table;
            table = new double[11, NoBeams];
            for (int i = 0; i < NoBeams; i++)
            {
                Console.WriteLine("Enter Beam {0} effective slab width, bslab (in) : ", i + 1);
                double bslab = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Enter Beam {0} structural slab thickness, tslab (in) : ", i + 1);
                double tslab = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Enter Beam {0} haunch width, bhaunch (in) : ", i + 1);
                double haunchWidth = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Enter Beam {0} haunch depth, thaunch (in) : ", i + 1);
                double haunchDepth = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Enter Beam {0} top rebar area, As1 (in^2) : ", i + 1);
                double Asteel1 = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Enter Beam {0} top rebar depth from top of slab, d1 (in) : ", i + 1);
                double distSteel1 = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Enter Beam {0} botom rebar area, As2 (in^2) : ", i + 1);
                double Asteel2 = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Enter Beam {0} bottom rebar depth from top of slab, d2 (in) : ", i + 1);
                double distSteel2 = Convert.ToDouble(Console.ReadLine()); 
                Console.WriteLine("Enter Beam {0} top flange width, bft (in) : ", i + 1);
                double bft = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Enter Beam {0} top flange thickness, tft (in) : ", i + 1);
                double tft = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Enter Beam {0} web depth, D (in) : ", i + 1);
                double D = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Enter Beam {0} web thickness, tw (in) : ", i + 1);
                double tw = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Enter Beam {0} bottom flange width, bfb (in) : ", i + 1);
                double bfb = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Enter Beam {0} bottom flange thickness, tfb (in) : ", i + 1);
                double tfb = Convert.ToDouble(Console.ReadLine());

                double area = CompProperties.BeamArea(bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, haunchDepth, haunchWidth);
                table[0, i] = area;
                double NA = CompProperties.NeutralAxis(bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
                table[1, i] = NA;
                double I = CompProperties.MomentOfIneria(bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
                table[2, i] = I;
                double Stop = CompProperties.ElastSectModTop(bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
                table[3, i] = Stop;
                double Sbot = CompProperties.ElastSectModBot(bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
                table[4, i] = Sbot;
                double PNA = CompProperties.PlastNeutralAxis(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
                table[5, i] = PNA;
                double PFt = CompProperties.PlastForceTop(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
                table[6, i] = PFt;
                double PFb = CompProperties.PlastForceBot(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
                table[7, i] = PFb;
                double PCGtop = CompProperties.PNAtoTopCG(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
                table[8, i] = PCGtop;
                double PCGBot = CompProperties.PNAtoBotCG(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
                table[9, i] = PCGBot;                 
                double Mp = CompProperties.PlastMoment(Fy, fc, fs, bft, tft, D, tw, bfb, tfb, bslab, tslab, n, Asteel1, Asteel2, distSteel1, distSteel2, haunchDepth, haunchWidth);
                table[10, i] = Mp;
            }
            for (int j = 0; j < NoBeams; j++)
            {
                Console.WriteLine("*********** BEAM {0} SECTION PROPERTIES***********", j + 1);
                Console.WriteLine("Area of Beam is {0} in^2", Math.Round(table[0, j], 2));
                Console.WriteLine("Elastic Neutral Axis is {0} in", Math.Round(table[1, j], 2));
                Console.WriteLine("Elastic Moment of Inertia is {0} in^4", Math.Round(table[2, j], 2));
                Console.WriteLine("Elastic Section Modulus of Top Flange is {0} in^3", Math.Round(table[3, j], 2));
                Console.WriteLine("Elastic Section Modulus of Bottom Flange is {0} in^3", Math.Round(table[4, j], 2));
                Console.WriteLine("Plastic Neutral Axis is {0} in", Math.Round(table[5, j], 2));
                Console.WriteLine("Force Above PNA is {0} kips", Math.Round(table[6, j], 2));
                Console.WriteLine("Distance to Force Above PNA is {0} in", Math.Round(table[8, j], 2));
                Console.WriteLine("Force Below PNA is {0} kips", Math.Round(table[7, j], 2));
                Console.WriteLine("Distance to Force Below PNA is {0} in", Math.Round(table[9, j], 2));
                Console.WriteLine("Plastic Moment is {0} kip-ft", Math.Round(table[10, j], 2));
                Console.WriteLine("");
            }
            Console.Read();
        }
    }
}
