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
        private void button1_Click(object sender, EventArgs e)
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
                /*0  */ char[] ChunkID = new char[4] { 'R', 'I', 'F', 'F' };
                /*(22) */  short NumChannels = 1;
                /*(32) */ short blockAlign = BITS_PER_SAMPLE / 8;
                
                /*(40) */ int Subchunk2Size = SAMPLE_RATE * NumChannels * blockAlign;
                /*4  */ int ChunkSize = 36 + Subchunk2Size;
                /*8  */ char[] Format = new char[4] { 'W', 'A', 'V', 'E' };

            //The "WAVE" format consists of two subchunks: "fmt " and "data":

            //~~~~~~~~ FMT Subchunk ~~~~~~~~\\

            //The "fmt " subchunk describes the sound data's format:

                /*12 */ char[] Subchunk1ID = new char[4] {'f','m','t',' '};
                /*16 */ int Subchunk1Size = 16;
                /*20 */ short AudioFormat = 1;
                /*22 */ //NumChannels
                /*24 */ int SampleRate = SAMPLE_RATE;
                /*28 */ int ByteRate = SampleRate*NumChannels*blockAlign;
                /*32 */ //Block Align
                /*34 */ short BitsPerSample = BITS_PER_SAMPLE;
           
            //~~~~~~~~ DATA Subchunk ~~~~~~~~\\

            //The "data" subchunk contains the size of the data and the actual sound:

                /*36 */char[] Subchunk2ID = new char[4] {'d','a','t','a'};
                /*40 */ //Subchunk2Size
                /*44 */ byte[] Data = binarywave;




            byte[] bufferRead = new byte[43];
            //---------------------------Write the .wav pararmeters through the memory stream and play them ---------------------------\\
            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream)) {
                binaryWriter.Write(ChunkID);
                binaryWriter.Write(ChunkSize);
                binaryWriter.Write(Format);
                binaryWriter.Write(Subchunk1ID);
                binaryWriter.Write(Subchunk1Size);
                binaryWriter.Write(AudioFormat);
                binaryWriter.Write(NumChannels);
                binaryWriter.Write(SampleRate);
                binaryWriter.Write(ByteRate);
                binaryWriter.Write(blockAlign);
                binaryWriter.Write(BitsPerSample);
                binaryWriter.Write(Subchunk2ID);
                binaryWriter.Write(Subchunk2Size);
                binaryWriter.Write(Data);
                memoryStream.Position = 0;
                new SoundPlayer(memoryStream).Play();

            }


        }

        /*private void button1_Click(object sender, EventArgs e)
        {

        }*/
    }
}
