using System;
using System.Collections.Generic;
using System.Text;

namespace SyntCore
{
    public class Sequencer
    {
        private int sampleRate;
        private int phaseStep;
        private int endPhase;
        private SeqStage[] seqStages;
        public int[] ReturnValue { get; set; }
        public int Tempo
        {
            set
            {
                var tempo = (float)value;
                tempo /= 60;
                var sampleRateFloat = (float)sampleRate;
                endPhase = (int)(sampleRateFloat / tempo);
            }
        }
        public Sequencer(int sampleRate)
        {
            ReturnValue = new int[9];
            this.sampleRate = sampleRate;
            phaseStep = 0;
            seqStages = new SeqStage[9];
            for (int i = 0; i < 9; i++)
            {
                seqStages[i] = new SeqStage
                {
                    Function = false,
                    Q = false
                };

            }
            seqStages[8].Q = true;
        }
        public void Tick()
        {
            phaseStep++;
            if (phaseStep > endPhase)
            {
                phaseStep = 0;
                ReturnValue = ClockCount();
            }
        }
        private int[] ClockCount()
        {
            bool loadFirstStage = false;

            // scan
            for (int f = 7; f > -1; f--)
            {
                if (seqStages[f].Q)
                {
                    if (seqStages[f].Function) // advanced
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
                    if (seqStages[f].Function) // advanced
                    {
                        //nodes[f].CarryNotSent = true;
                    }
                }

            }
            if (seqStages[8].Q)
            {
                loadFirstStage = true;
                seqStages[8].Q = false;
                seqStages[8].CarryNotSent = false;
            }
            if (loadFirstStage)
            {
                seqStages[0].Q = true;
            }
            int[] returnV = new int[8];
            for(int f = 0; f < 8; f++)
            {
                returnV[f] = seqStages[f].Q ? 1 : 0;
            }
            return returnV;
        }
        private bool CheckAdvancedNode(int i)
        {
            if (seqStages[i].CarryNotSent)
            {
                seqStages[i].CarryNotSent = false;
                return true; // Sent back carry and no forwarding
            }
            else
            {
                if (i > 0)
                {
                    if (seqStages[i - 1].Q == true)
                    {
                        seqStages[i - 1].Q = false;
                        seqStages[i].Q = false;
                        seqStages[i + 1].Q = true;
                        seqStages[i + 1].CarryNotSent = true;
                    }
                }
                else
                {
                    if (seqStages[8].Q == true)
                    {
                        seqStages[8].Q = false;
                        seqStages[0].Q = false;
                        seqStages[1].Q = true;
                        seqStages[1].CarryNotSent = true;
                    }
                }

            }
            return false;
        }
        private bool ClockSimple(int f)
        {
            seqStages[f].Q = false;
            if (seqStages[f + 1].Function)
            {
                if (seqStages[f + 1].Q)
                {
                    seqStages[f + 1].Q = false;
                }
                else
                {
                    seqStages[f + 1].Q = true;
                    seqStages[f + 1].CarryNotSent = true;
                }
            }
            else
            {
                seqStages[f + 1].Q = true;
            }

            if (f > 6)
            {
                return true;
            }
            return false;
        }
    }
}
