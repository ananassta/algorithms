using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace algo
{
    class Program
    {
        static int best_cost = 100000;
        static List<int> best_cut = new List<int>();
        static double mincut(int n, int[,] g) {
            int[] v=new int[n];
            	for (int i = 0; i<n; ++i)
                		v[i]=i;
                int[] w = new int[n];
                bool[] exist = new bool[n];
                bool[] in_a = new bool[n];
                for (int j=0;j<n;j++){
                    exist[j]=true;
                }
                for (int ph = 0; ph<n - 1; ++ph) {
                    for (int j=0;j<n;j++)
                        in_a[j]=false;
                    for (int j=0;j<n;j++)
                        w[j]=0;
                    for (int it = 0; it<n - ph; ++it) {
                        int prev=it;
                        int sel = -1;
                        for (int i = 0; i<n; ++i)
                            if (exist[i] && !in_a[i] && (sel == -1 || w[i] > w[sel]))
                                sel = i;
                        if (it == n - ph - 1) {
                            if (w[sel] < best_cost) {
                            	best_cost = w[sel]; best_cut.Add(v[sel]);
                        	}
                            string[] words = new string[2];
                            words[0] = v[prev].ToString();
                            words[1]= v[sel].ToString();
                            string singleString = String.Join("", words);
                        	v[prev]=Convert.ToInt32(singleString);
                        	for (int i = 0; i<n; ++i)
                        		g[prev,i] = g[i,prev] += g[sel,i];
                        	exist[sel] = false;
                        }
                        else {
                        	in_a[sel] = true;
                        	for (int i = 0; i < n; ++i) {
                        		w[i] += g[sel,i];
                        	}
                        	prev = sel;
                        }
                    }
                }
            return best_cost; // стоимость минимального разреза
        }

        static void Main(string[] args)
        {

            //функция построения матрицы смежности------------
            Random ran = new Random();
            int[,] BuildMatrix(int N)
            {
              int[,] matrix = new int[N, N];
              for (int i = 0; i < N; i++)
              {
                matrix[i, i] = 0;
                for (int j = i + 1; j < N; j++)
                {
                  matrix[i, j] = ran.Next(0, 3);
                  if (matrix[i,j]!=0) matrix[i,j]=ran.Next(1,11);
                  matrix[j, i] = matrix[i, j]; // обратный порядок индексов
                }
              }
              return matrix;
            }//-----------------------

            Stopwatch work_time = new Stopwatch();
            int[] mass_N=new int[10];
            for (int i=0;i<10;i++)
               mass_N[i]=100*(i+1);
            double[] mass_T=new double[10];
            for (int n_i=0;n_i<10;n_i++){
                int n=mass_N[n_i];
            
            int[,] mass = new int[n, n];
            mass = BuildMatrix(n);
            
            //получаем количество ребер
            int si=1;
            int number_edges=0;
            for (int i=0; i<(n-1);i++){
               for (int j=si;j<n;j++){
                   if (mass[i,j]!=0)
                   number_edges++;
               }
               si++;
            }
            int m=number_edges;
            //вспомогательная матрица смежности для перезаписи mass
            int[,] mass_vspm = new int[n,n];
            for (int i=0;i<n;i++){
               for (int j=0;j<n;j++){
                    mass_vspm[i,j]=mass[i,j];
               }
            }

        	double sum = 0;
        	double res = 0;
        	for (int i = 0; i < 20; i++) {
                work_time.Start();
        		double bb = mincut(n, mass);
                work_time.Stop();
                TimeSpan ts = work_time.Elapsed;
                double ft=ts.TotalSeconds;
        		res = bb;
                sum=sum+ft;
        		for (int k = 0; k < n; k++) {
        			for (int j = 0; j < n; j++) {
        				mass[k,j] = mass_vspm[k,j];
        			}
        		}
        	}
        	double res_time = sum / 20;
            mass_T[n_i]=res_time;
            Console.WriteLine($"{mass_N[n_i]}");
            Console.WriteLine($"{res}");
            Console.Write("time: ");
            Console.WriteLine($"{res_time}");
            best_cost=100000;
            best_cut.Clear();
            }
        }
        
    }
}
