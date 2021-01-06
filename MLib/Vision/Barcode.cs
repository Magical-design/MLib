using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLib
{
    public class Barcode
    {
        static HTuple mhv_DataCodeHandleStandard = new HTuple(), mhv_DataCodeHandleEnhanced = new HTuple(), mhv_DataCodeHandleMaximum = new HTuple();
        static List<HTuple> hv_DataCodeHandleStandard = new List<HTuple>(), hv_DataCodeHandleEnhanced = new List<HTuple>();
        static List<HTuple> hv_DataCodeHandleMaximum = new List<HTuple>();
        static HTuple hv_Index = null;
        static int first =0;

        static HTuple hv_SymbolType = new HTuple();

        public static string GetBarCode(HObject ho_Image,ref HObject ho_SymbolXLDs)
        {
            if(first==0)
            {
                hv_SymbolType = hv_SymbolType.TupleConcat("Data Matrix ECC 200");
                hv_SymbolType = hv_SymbolType.TupleConcat("QR Code");
                hv_SymbolType = hv_SymbolType.TupleConcat("Micro QR Code");
                hv_SymbolType = hv_SymbolType.TupleConcat("PDF417");
                hv_SymbolType = hv_SymbolType.TupleConcat("Aztec Code");

                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_SymbolType.TupleLength()
           )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    //Create a model of the current symbol type for each
                    //default setting standard, enhanced, and maximum.
                    HOperatorSet.CreateDataCode2dModel(hv_SymbolType.TupleSelect(hv_Index), "default_parameters",
                            "standard_recognition", out mhv_DataCodeHandleStandard);
                    HOperatorSet.CreateDataCode2dModel(hv_SymbolType.TupleSelect(hv_Index), "default_parameters",
                        "enhanced_recognition", out mhv_DataCodeHandleEnhanced);
                    HOperatorSet.CreateDataCode2dModel(hv_SymbolType.TupleSelect(hv_Index), "default_parameters",
                        "maximum_recognition", out mhv_DataCodeHandleMaximum);
                    //Find and decode the data codes with each
                    //data code model and measure the runtime
                    //
                    hv_DataCodeHandleStandard.Add(mhv_DataCodeHandleStandard);
                    hv_DataCodeHandleEnhanced.Add(mhv_DataCodeHandleEnhanced);
                    hv_DataCodeHandleMaximum.Add(mhv_DataCodeHandleMaximum);
                }
                first = 1;
            }



            int i;
            HOperatorSet.GenEmptyObj(out ho_SymbolXLDs);


            HTuple hv_ResultHandles = new HTuple();
            HTuple hv_DecodedDataStrings = new HTuple();


            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_SymbolType.TupleLength()
     )) - 1); hv_Index = (int)hv_Index + 1)
            {



                for (i = 0; i < 3; i++)
                {
                    ho_SymbolXLDs.Dispose();
                    hv_DecodedDataStrings = new HTuple();

                    switch (i)
                    {
                        case 0:
                            //Standard mode
                            HOperatorSet.FindDataCode2d(ho_Image, out ho_SymbolXLDs, hv_DataCodeHandleStandard[hv_Index],
                                new HTuple(), new HTuple(), out hv_ResultHandles, out hv_DecodedDataStrings);

                            break;
                        case 1:
                            //Enhanced mode
                            HOperatorSet.FindDataCode2d(ho_Image, out ho_SymbolXLDs, hv_DataCodeHandleEnhanced[hv_Index],
                                new HTuple(), new HTuple(), out hv_ResultHandles, out hv_DecodedDataStrings);
                            break;
                        case 2:
                            //Maximum mode
                            HOperatorSet.FindDataCode2d(ho_Image, out ho_SymbolXLDs, hv_DataCodeHandleMaximum[hv_Index],
                                new HTuple(), new HTuple(), out hv_ResultHandles, out hv_DecodedDataStrings);
                            break;
                        default:
                            break;
                    }

                    if (hv_DecodedDataStrings.TupleLength() != 0)
                    {
                        ho_Image.Dispose();
                        return hv_DecodedDataStrings.S;
                    }

                }

            }

            //Clear the 2d data code models
            //HOperatorSet.ClearDataCode2dModel(hv_DataCodeHandleStandard);
            //HOperatorSet.ClearDataCode2dModel(hv_DataCodeHandleEnhanced);
            //HOperatorSet.ClearDataCode2dModel(hv_DataCodeHandleMaximum);
            ho_Image.Dispose();
            return null;
        }

    }
}
