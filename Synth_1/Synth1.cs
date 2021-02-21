using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace Synth_1
{
    public partial class Synth1 : Form
    {

        private const int SAMPLE_RATE = 44100;
        private const short BITS_PER_SAMPLE = 16;

        public Synth1()
        {
            InitializeComponent();
        }

        private void Synth1_Load(object sender, EventArgs e)
        {

        }
        private void Synth1_KeyDown(object sender, EventArgs e) { 
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            //define wave storage over 1 second
            short[] wave = new short[SAMPLE_RATE];
            byte[] binarywave = new byte[SAMPLE_RATE * sizeof(short)]; // wave needs to be converted to a single byte based array to write to the memroy stream using the binary writer class
            float f = 440f;
            float A = short.MaxValue;

            for (int i = 0; i < SAMPLE_RATE; i++){
                wave[i] = Convert.ToInt16(A * Math.Sin(((2*Math.PI*f)/SAMPLE_RATE)*i));
            }

            Buffer.BlockCopy(wave, 0, binarywave, 0, wave.Length*sizeof(short)); // splits every short in the 'wave' array into 2 bytes and writes them into the 'binarywave' array


            //---------------------------Setting up .wav file parameters---------------------------\\

            //The canonical WAVE format starts with the RIFF header:

                /*Offset*/

                /*0  */ byte[] ChunkID = new byte[4] { (byte)'R', (byte)'I', (byte)'F', (byte)'F' };
                /*(22) */  byte[] NumChannels = BitConverter.GetBytes((short)1);
                /*(32) */ byte[] blockAlign = BitConverter.GetBytes((short)(BITS_PER_SAMPLE / 8));
                /*(40) */ byte[] Subchunk2Size = BitConverter.GetBytes((int)(SAMPLE_RATE * BitConverter.ToInt16(NumChannels,0) * BitConverter.ToInt16(blockAlign,0)));
                /*4  */ byte[] ChunkSize = BitConverter.GetBytes((int)(36 + BitConverter.ToInt32(Subchunk2Size,0)));
                /*8  */ byte[] Format = new byte[4] { (byte)'W', (byte)'A', (byte)'V', (byte)'E' };

            //The "WAVE" format consists of two subchunks: "fmt " and "data":

            //~~~~~~~~ FMT Subchunk ~~~~~~~~\\

            //The "fmt " subchunk describes the sound data's format:
                
                /*12 */ byte[] Subchunk1ID = new byte[4] {(byte)'f',(byte)'m',(byte)'t',(byte)' '};
                /*16 */ byte[] Subchunk1Size = BitConverter.GetBytes((int)16);
                /*20 */ byte[] AudioFormat = BitConverter.GetBytes((short)1);
                /*22 */ //NumChannels
                /*24 */ byte[] SampleRate = BitConverter.GetBytes((int)SAMPLE_RATE);
                /*28 */ byte[] ByteRate = BitConverter.GetBytes((int)(BitConverter.ToInt32(SampleRate,0)* BitConverter.ToInt16(NumChannels, 0) * BitConverter.ToInt16(blockAlign, 0)));
                /*32 */ //Block Align
                /*34 */ byte[] BitsPerSample = BitConverter.GetBytes((short)BITS_PER_SAMPLE);
           
            //~~~~~~~~ DATA Subchunk ~~~~~~~~\\

            //The "data" subchunk contains the size of the data and the actual sound:

                /*36 */byte[] Subchunk2ID = new byte[4] {(byte)'d',(byte)'a',(byte)'t',(byte)'a'};
                /*40 */ //Subchunk2Size
                /*44 */ byte[] Data = binarywave;

                //Combine the wave formate parameters into a single array to be written to the Memory Stream via the Binary Writer
                byte[] wav = ChunkID.Concat(ChunkSize).Concat(Format).Concat(Subchunk1ID).Concat(Subchunk1Size).Concat(AudioFormat).Concat(NumChannels).Concat(SampleRate)
                                    .Concat(ByteRate).Concat(blockAlign).Concat(BitsPerSample).Concat(Subchunk2ID).Concat(Subchunk2Size).Concat(Data).ToArray();



            //byte[] bufferRead = new byte[43];
            //---------------------------Write the .wav pararmeters through the memory stream and play them ---------------------------\\
            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream)) {
                binaryWriter.Write(wav);
                memoryStream.Position = 0;
                new SoundPlayer(memoryStream).Play();

            }


        }
    }
}
