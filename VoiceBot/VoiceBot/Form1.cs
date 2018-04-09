using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Diagnostics;
using System.IO;

namespace VoiceBot
{
    public partial class Form1 : Form
    {

        SpeechSynthesizer s = new SpeechSynthesizer();

        Boolean wake = true;
        Boolean search = false;
        Boolean shutdown = false;


        String name = "James";
        

        Grammar gr = new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"H:\Programs\VB\Projects\VoiceBot\Voice Bot Commands\commands.txt"))));

        //Grammar searchGr = new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"H:\Programs\VB\Projects\VoiceBot\Voice Bot Commands\words.txt"))));

        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();

        public Form1()
        {

            try
            {
                rec.RequestRecognizerUpdate();
                rec.LoadGrammar(gr);
                rec.SpeechRecognized += rec_speechRecognized;
                rec.SetInputToDefaultAudioDevice();
                rec.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch { return; }


            InitializeComponent();


            say("Hello " + name + " , my name is VoiceBot");
        }








        //variety responses

        String[] greetings = new String[] {"Hi","Hello","Hi, how are you?"};

        public String greetings_action()
        {
            Random r = new Random();

            return greetings[r.Next(greetings.Length)];
        }

        String[] questioningResponses = new String[] { "Yes?", "Yes, what can I do for you?" };

        public String questioningResponses_Action()
        {
            Random r = new Random();

            return questioningResponses[r.Next(questioningResponses.Length)];
        }

        String[] petNames = new String[] { "my feathered friend", "stupid", "puny mortal" };

        public String petNames_Action()
        {
            Random r = new Random();

            return petNames[r.Next(petNames.Length)];
        }






        //commands

        private void rec_speechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            String r = e.Result.Text;

            listBox1.Items.Add(r);



            //search
            if (search)
            {
                Process.Start("http://www.google.com/#q="+r);
                search = false;
                rec.UnloadAllGrammars();
                rec.LoadGrammar(gr);
            }
            else
            {
                //wake and sleep

                if (r == "wake up")
                {
                    if (wake)
                    {
                        say("I'm already awake " + petNames_Action());
                    }
                    else
                    {
                        say(questioningResponses_Action());
                        wake = true;
                    }
                    
                }

                if (r == "sleep")
                {
                    if (!wake)
                    {
                        say("I'm already asleep " + petNames_Action());
                    }
                    else
                    {
                        say("zzzz");
                        wake = false;
                    }
                }



                //commands that work while not asleep


                if (wake == true)
                {

                    //chatter


                    if (r == "how are you")
                    {
                        say("Great, and you " + petNames_Action() + "?");
                    }

                    if (r == "what's my name")
                    {
                        say("Your name is " + name + " " + petNames_Action());
                    }


                    //useful commands

                    if (r == "close")
                    {
                        say("Goodbye " + petNames_Action());
                        Application.Exit();
                    }

                    //spotify controls

                    if (r == "spotify")
                    {
                        Process.Start(@"C:\Users\James\AppData\Roaming\Spotify\Spotify.exe");
                    }

                    if (r == "play" || r == "pause")
                    {
                        SendKeys.Send(" ");
                    }

                    if (r == "previous")
                    {
                        SendKeys.Send("^{LEFT}");
                        SendKeys.Send("^{LEFT}");
                    }

                    if (r == "next")
                    {
                        SendKeys.Send("^{RIGHT}");
                    }

                    if (r == "turn it up")
                    {
                        SendKeys.Send("^{UP}");
                        SendKeys.Send("^{UP}");
                    }

                    if (r == "turn it down")
                    {
                        SendKeys.Send("^{DOWN}");
                        SendKeys.Send("^{DOWN}");
                    }



                    //open and close programs


                    if (r == "open google")
                    {
                        Process.Start("chrome.exe");
                    }

                    //search google
                    //This commented out because its easier not to use it as the thing recognises words that arent there sometimes
                    //if (r == "search for")
                    //{
                    //    rec.UnloadAllGrammars();
                    //    rec.LoadGrammar(searchGr);
                    //    search = true;
                    //}



                    //other

                    if (r == "shutdown")
                    {
                        if (shutdown)
                        {
                            say("Goodbye " + petNames_Action());
                            Process.Start("shutdown", "/s /t 0");
                            Application.Exit();
                        }
                        else
                        {
                            shutdown = true;
                            say("Say shutdown again to shutdown the computer. Otherwise say no");
                        }
                    }

                    if (r == "no")
                    {
                        if (shutdown)
                        {
                            shutdown = false;
                            say("shutdown stopped");
                        } 
                    }




                    //easter egg
                    if (r == "jingle cats")
                    {
                        say("MEOW!, MEOW!, MEOW!, MEOW!, MEOW!, MEOW!, MEOW!, MEOW!, MEOW!, MEOW!");
                    }

                }


            }

        }






        public void say(String h)
        {
            s.Speak(h);
            listBox2.Items.Add(h);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
