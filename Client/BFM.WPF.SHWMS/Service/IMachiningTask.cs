using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFM.WPF.SHWMS.Service
{
    public interface IMachiningTask
    {
        void GenerateMachiningTask_Piece1();
        void GenerateMachiningTask_Piece2();
        void GenerateMachiningTask_Piece3();
        void GenerateMachiningTask_Piece4();

        bool IsAssembleFinished();

        void Btn_AssemblyClick(string LaserPicName);
    }
}
