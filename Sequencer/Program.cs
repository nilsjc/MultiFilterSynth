using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SyntCore;
using System;
using System.Threading;

namespace Sequencer
{
    class Program
    {
        static IWavePlayer player;
        static AudioProvider audioProvider;
        static Node[] nodes;
        static void Main(string[] args)
        {
            var seq = new SyntCore.Sequencer(44100);
            seq.Tempo = 30;
            audioProvider = new AudioProvider();
            var waveOutEvent = new WaveOutEvent();
            waveOutEvent.NumberOfBuffers = 2;
            waveOutEvent.DesiredLatency = 100;
            player = waveOutEvent;
            var stwp = new SampleToWaveProvider(audioProvider);
            player.Init(stwp);
            player.Play();
            Console.WriteLine("Sequencer");
            Console.WriteLine("--------");
            Console.WriteLine("--------");
            
            bool run = true;
            nodes = new Node[9];
            for (int i = 0; i < 9; i++)
            {
                nodes[i] = new Node
                {
                    Function = false,
                    Q = false
                };

            }
            // Modulation matrix
            audioProvider.SetDest(0, 0b1111110);
            audioProvider.SetDest(1, 0b100);
            audioProvider.SetDest(2, 0b1000);
            audioProvider.SetDest(3, 0b10010);
            audioProvider.SetDest(4, 0b1100000);
            audioProvider.SetDest(5, 0b1111111);
            audioProvider.SetDest(6, 0b1111000);
            audioProvider.SetDest(7, 0b1000000);

            nodes[8].Q = true;
            
            nodes[0].Function = false;
            nodes[1].Function = false;
            nodes[2].Function = false;
            nodes[3].Function = false;
            nodes[4].Function = false;
            nodes[5].Function = true;
            nodes[6].Function = false;
            //nodes[7].Function = true;
            audioProvider.SetFreq(38, 0);   //48
            audioProvider.SetFreq(8, 1);    // 325
            audioProvider.SetFreq(20, 2);  //100
            audioProvider.SetFreq(8, 3); // 1140
            audioProvider.SetFreq(50, 4);
            audioProvider.SetFreq(8, 5);
            audioProvider.SetFreq(18, 6);
            audioProvider.SetFreq(8, 7);
            

            while (run)
            {
                for(int x = 0; x < 8; x++)
                {
                    SetLevel(x);
                    StatusWrite(x);
                }
                Console.SetCursorPosition(0, 5);
                Count();
                Thread.Sleep(200);
            }
        }
        static void SetLevel(int x)
        {
            if (nodes[x].Q)
            {
                audioProvider.SetVol(1.0f, x);
            }
            else
            {
                audioProvider.SetVol(0, x);
            }
            
        }

        static void StatusWrite(int x)
        {
            Console.SetCursorPosition((x * 2), 5);
            Console.Write($"{x} ");
            Console.SetCursorPosition(x, 1);
            if (nodes[x].Q)
            {
                Console.Write("X");
            }
            else
            {
                Console.Write("-");
            }
        }
        
        static void Count()
        {
            bool loadFirstStage = false;

            // scan
            for (int f = 7; f > -1; f--)
            {
                if (nodes[f].Q)
                {
                    if (nodes[f].Function) // advanced
                    {
                        loadFirstStage = CheckAdvancedNode(f);
                    }
                    else // simple
                    {
                        loadFirstStage = ClockSimple(f);
                    }
                }
                else
                {
                    if (nodes[f].Function) // advanced
                    {
                        //nodes[f].CarryNotSent = true;
                    }
                }
                
            }
            if (nodes[8].Q)
            {
                loadFirstStage = true;
                nodes[8].Q = false;
                nodes[8].CarryNotSent = false;
            }  
            if (loadFirstStage)
            {
                nodes[0].Q = true;
            }
        }
        static bool CheckAdvancedNode(int i)
        {
            if (nodes[i].CarryNotSent)
            {
                nodes[i].CarryNotSent = false;
                return true; // Sent back carry and no forwarding
            }
            else
            {
                if(i > 0)
                {
                    if(nodes[i - 1].Q == true)
                    {
                        nodes[i - 1].Q = false;
                        nodes[i].Q = false;
                        nodes[i + 1].Q = true;
                        nodes[i + 1].CarryNotSent = true;
                    }
                }
                else
                {
                    if (nodes[8].Q == true)
                    {
                        nodes[8].Q = false;
                        nodes[0].Q = false;
                        nodes[1].Q = true;
                        nodes[1].CarryNotSent = true;
                    }
                }
                
            }
            return false;
        }
        static bool ClockSimple(int f)
        {
            nodes[f].Q = false;
            if(nodes[f + 1].Function)
            {
                if(nodes[f + 1].Q)
                {
                    nodes[f + 1].Q = false;
                }
                else
                {
                    nodes[f + 1].Q = true;
                    nodes[f + 1].CarryNotSent = true;
                }
            }
            else
            {
                nodes[f + 1].Q = true;
            }
            
            if (f > 6)
            {
                return true;
            }
            return false;
        }
    }
    class Node
    {
        public bool Function { get; set; }  // 0 = normal 1 = counter
        public bool CarryNotSent { get; set; }
        public bool Q { get; set; }
        public bool Input { get; set; }
    }
}
