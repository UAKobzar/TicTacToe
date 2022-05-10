// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using TicTacToe;

Board.Calculate();
var teacher = new EvolutionTeacher();

teacher.Teach(10);

var weigths = teacher.BestNetwork.Save();
//System.IO.File.WriteAllText("../saved/weights", Newtonsoft.Json.JsonConvert.SerializeObject(weigths));

//var weights = Newtonsoft.Json.JsonConvert.DeserializeObject<List<double[,]>>(System.IO.File.ReadAllText(@"D:\progs\TicTacToe\saved\weghts"));
var network = teacher.BestNetwork;
network.Load(weigths);

var nnplayer = new NNPlayer(teacher.BestNetwork);
var humanPlayer = new HumanPlayer();

var agent = new TicTacToeAgent(nnplayer, humanPlayer, true);
agent.Play();
agent.Swap();
agent.Play();



Console.ReadKey();
