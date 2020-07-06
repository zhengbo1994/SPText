using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPText.Common
{
    public static class AlgorithmHelper
    {
        #region  DynamicPlanningDemo
        public static void Show0()
        {
            #region Fibonacci
            //int n = 5;
            //{
            //    {
            //        Stopwatch stopwatch = new Stopwatch();
            //        stopwatch.Start();
            //        long lResult = RecursionFibonacci(n);
            //        stopwatch.Stop();
            //        Console.WriteLine($"RecursionFibonacci {n}  Time{stopwatch.ElapsedMilliseconds}");
            //    }
            //    {
            //        Stopwatch stopwatch = new Stopwatch();
            //        stopwatch.Start();
            //        long lResult = DynamicFibonacci(n);
            //        stopwatch.Stop();
            //        Console.WriteLine($"DynamicFibonacci {n}  Time{stopwatch.ElapsedMilliseconds}");
            //    }
            //    {
            //        Stopwatch stopwatch = new Stopwatch();
            //        stopwatch.Start();
            //        long lResult = DynamicFibonacciWithoutArray(n);
            //        stopwatch.Stop();
            //        Console.WriteLine($"DynamicFibonacciWithoutArray {n}  Time{stopwatch.ElapsedMilliseconds}");
            //    }
            //}
            #endregion

            #region 公共字符串
            //string wordLeft = "eleven";
            //string wordRight = "seven";// "secen";// "sevem";
            //string result = FindLongCommonSubString(wordLeft, wordRight);

            //if (string.IsNullOrWhiteSpace(result))
            //{
            //    Console.WriteLine("没有共同字符串");
            //}
            //else
            //{
            //    Console.WriteLine("最长共同字符串: " + result);
            //}
            #endregion

            #region MyRegion
            Package();
            #endregion
        }

        #region Fibonacci
        /// <summary>
        /// 递归--0 1 1 2 3 5 8
        /// </summary>
        /// <param name="n">从1开始</param>
        /// <returns></returns>
        public static long RecursionFibonacci(int n)
        {
            if (n < 2)
            {
                return n;
            }
            else
            {
                return RecursionFibonacci(n - 1) + RecursionFibonacci(n - 2);
            }
        }
        /// <summary>
        /// 动态规划-0 1 1 2 3 5 8
        /// </summary>
        /// <param name="n">从1开始</param>
        /// <returns></returns>
        public static long DynamicFibonacci(int n)
        {
            int[] totalArray = new int[n];
            if (n == 1 || n == 2)
            {
                return 1;
            }
            else
            {
                totalArray[1] = 1;
                totalArray[2] = 2;
                for (int i = 3; i <= n - 1; i++)
                {
                    totalArray[i] = totalArray[i - 1] + totalArray[i - 2];
                }
            }
            return totalArray[n - 1];
        }
        /// <summary>
        /// 动态规划-0 1 1 2 3 5 8
        /// </summary>
        /// <param name="n">从1开始</param>
        /// <returns></returns>
        public static long DynamicFibonacciWithoutArray(int n)
        {
            long last = 1;
            long nextLast = 1;
            long result = 1;
            for (int i = 2; i <= n - 1; i++)
            {
                result = last + nextLast;
                nextLast = last;
                last = result;
            }
            return result;
        }
        #endregion

        #region 公共字符串
        public static string FindLongCommonSubString(string wordLeft, string wordRight)
        {
            string[] warrayLeft = new string[wordLeft.Length];
            string[] warrayRight = new string[wordRight.Length];
            string subString;
            int[,] larray = new int[wordLeft.Length, wordRight.Length];
            CompareString(wordLeft, wordRight, warrayLeft, warrayRight, larray);
            Console.WriteLine();
            ShowArray(larray);
            subString = GetString(larray, warrayLeft);
            Console.WriteLine();
            Console.WriteLine("The strings are: " + wordLeft + " " + wordRight);

            return subString;
        }

        /// <summary>
        /// 分组比较
        /// </summary>
        /// <param name="wordLeft"></param>
        /// <param name="wordRight"></param>
        /// <param name="wordArrayLeft"></param>
        /// <param name="wordArrayRight"></param>
        /// <param name="arr"></param>
        private static void CompareString(string wordLeft, string wordRight, string[] wordArrayLeft, string[] wordArrayRight, int[,] arr)
        {
            int lenLeft = wordLeft.Length;
            int lenRight = wordRight.Length;
            for (int k = 0; k <= wordLeft.Length - 1; k++)
            {
                wordArrayLeft[k] = wordLeft.Substring(k, 1);
            }
            for (int k = 0; k <= wordRight.Length - 1; k++)
            {
                wordArrayRight[k] = wordRight.Substring(k, 1);
            }
            for (int i = lenLeft - 1; i >= 0; i--)
            {
                for (int j = lenRight - 1; j >= 0; j--)
                {
                    if (wordArrayLeft[i] == wordArrayRight[j])
                    {
                        if (i == lenLeft - 1 || j == lenRight - 1)//最右边
                        {
                            arr[i, j] = 1;
                        }
                        else
                        {
                            arr[i, j] = 1 + arr[i + 1, j + 1];
                        }
                        //arr[i, j] = 1;
                    }
                    else
                    {
                        arr[i, j] = 0;
                    }
                }
            }
        }
        /// <summary>
        /// 寻找最长字符串
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="wordArrayLeft"></param>
        /// <returns></returns>
        private static string GetString(int[,] arr, string[] wordArrayLeft)
        {
            string subString = "";
            int max = 0;
            int leftIndex = 0;
            for (int i = 0; i <= arr.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= arr.GetUpperBound(1); j++)
                {
                    if (arr[i, j] > max)
                    {
                        max = arr[i, j];
                        leftIndex = i;
                    }
                }
            }
            for (int i = 0; i < max; i++)
            {
                subString += wordArrayLeft[leftIndex + i];
            }
            return subString;
        }
        /// <summary>
        /// 二维数组展示
        /// </summary>
        /// <param name="arr"></param>
        private static void ShowArray(int[,] arr)
        {
            Console.WriteLine("展示比对结果");
            for (int row = 0; row <= arr.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= arr.GetUpperBound(1); col++)
                {
                    Console.Write(arr[row, col] + " ");
                }
                Console.WriteLine();
            }
        }
        #endregion

        #region 背包问题
        public static void Package()
        {
            int capacity = 16;
            int[] size = new int[] { 3, 4, 7, 8, 9 };//财宝尺寸
            int[] values = new int[] { 4, 5, 10, 11, 13 };//财宝价值
            int[] totleValue = new int[capacity + 1];//价值组--用来保存当时的最大价值

            for (int j = 0; j <= values.Length - 1; j++)//假如只有一件宝物--然后再慢慢增加
            {
                for (int i = 0; i <= capacity; i++)//假如只有一个单位的空间--然后再慢慢增加
                {
                    if (i >= size[j])
                    {
                        if (totleValue[i] < (totleValue[i - size[j]] + values[j]))
                        {
                            totleValue[i] = totleValue[i - size[j]] + values[j];
                        }
                    }
                }
            }
            Console.WriteLine("The Max value is: " + totleValue[capacity]);
        }
        #endregion

        #endregion

        #region  GreedyAlgorithmDemo
        public static void Show1()
        {
            #region 找零问题
            {
                double origAmount = 0.63;//原始总金额
                double toChange = origAmount;
                double remainAmount = 0.0;
                int[] coins = new int[4];//4种面值 1-5-10-25
                MakeChange(origAmount, remainAmount, coins);
            }
            #endregion

            #region 霍夫曼
            {
                string input;
                Console.Write("请输入一个字符串: ");
                input = Console.ReadLine();
                TreeList treeList = new TreeList(input);
                for (int i = 0; i < input.Length; i++)
                {
                    treeList.AddSign(input[i].ToString());//链表--计数
                }

                treeList.SortTree();
                while (treeList.Length() > 1)
                    treeList.MergeTree();
                TreeList.MakeKey(treeList.RemoveTree(), "");
                string newStr = TreeList.Translate(input);
                string[] signTable = treeList.GetSignTable();
                string[] keyTable = treeList.GetKeyTable();
                for (int i = 0; i <= signTable.Length - 1; i++)
                    Console.WriteLine(signTable[i] + ": " + keyTable[i]);

                Console.WriteLine("字符串长度 " + input.Length * 16 + " bits.");
                Console.WriteLine("压缩后长度 " + newStr.Length + " bits long.");
                Console.WriteLine("压缩后编码:" + newStr);
            }
            #endregion

            #region MyRegion
            {
                Carpet c1 = new Carpet("Frieze", 1.75F, 12);
                Carpet c2 = new Carpet("Saxony", 1.82F, 9);
                Carpet c3 = new Carpet("Shag", 1.5F, 13);
                Carpet c4 = new Carpet("Loop", 1.77F, 10);
                ArrayList rugs = new ArrayList();
                rugs.Add(c1);
                rugs.Add(c2);
                rugs.Add(c3);
                rugs.Add(c4);
                rugs.Sort();
                Knapsack k = new Knapsack(25);
                k.FillSack(rugs);
                Console.WriteLine(k.GetItems());
            }
            #endregion
        }

        #region 找零钱问题
        public static void MakeChange(double origAmount, double remainAmount, int[] coins)
        {
            if ((origAmount % 0.25) < origAmount)
            {
                coins[3] = (int)(origAmount / 0.25);
                remainAmount = origAmount % 0.25;
                origAmount = remainAmount;
            }
            if ((origAmount % 0.1) < origAmount)
            {
                coins[2] = (int)(origAmount / 0.1);
                remainAmount = origAmount % 0.1;
                origAmount = remainAmount;
            }
            if ((origAmount % 0.05) < origAmount)
            {
                coins[1] = (int)(origAmount / 0.05);
                remainAmount = origAmount % 0.05;
                origAmount = remainAmount;
            }
            if ((origAmount % 0.01) < origAmount)
            {
                coins[0] = (int)(origAmount / 0.01);
                remainAmount = origAmount % 0.01;
            }
            ShowChange(coins);
        }
        private static void ShowChange(int[] arr)
        {
            Console.WriteLine($"一共{arr.Length}硬币");
            if (arr[3] > 0)
                Console.WriteLine("Number of 25: " + arr[3]);
            if (arr[2] > 0)
                Console.WriteLine("Number of 10: " + arr[2]);
            if (arr[1] > 0)
                Console.WriteLine("Number of 5: " + arr[1]);
            if (arr[0] > 0)
                Console.WriteLine("Number of 1: " + arr[0]);
        }
        #endregion

        #region 霍夫曼
        public class Node
        {
            public HuffmanTree data;
            public Node link;
            public Node(HuffmanTree newData)
            {
                data = newData;
            }
        }
        public class TreeList
        {
            private int count = 0;
            private Node first = null;
            private static string[] signTable = null;
            private static string[] keyTable = null;
            public TreeList(string input)
            {
                List<char> list = new List<char>();
                for (int i = 0; i < input.Length; i++)
                {
                    if (!list.Contains(input[i]))
                        list.Add(input[i]);
                }
                signTable = new string[list.Count];
                keyTable = new string[list.Count];
            }
            public string[] GetSignTable()
            {
                return signTable;
            }
            public string[] GetKeyTable()
            {
                return keyTable;
            }
            public void AddLetter(string letter)
            {
                HuffmanTree hTemp = new HuffmanTree(letter);
                Node eTemp = new Node(hTemp);
                if (first == null)
                    first = eTemp;
                else
                {
                    eTemp.link = first;
                    first = eTemp;

                    count++;
                }
            }

            public void SortTree()
            {
                if (first != null && first.link != null)
                {
                    Node tmp1;
                    Node tmp2;
                    for (tmp1 = first; tmp1 != null; tmp1 = tmp1.link)
                        for (tmp2 = tmp1.link; tmp2 != null; tmp2 = tmp2.link)
                        {
                            if (tmp1.data.GetFreq() > tmp2.data.GetFreq())
                            {
                                HuffmanTree tmpHT = tmp1.data;
                                tmp1.data = tmp2.data;
                                tmp2.data = tmpHT;
                            }
                        }
                }
            }
            public void MergeTree()
            {
                if (!(first == null))
                    if (!(first.link == null))
                    {
                        HuffmanTree aTemp = RemoveTree();
                        HuffmanTree bTemp = RemoveTree();
                        HuffmanTree sumTemp = new HuffmanTree("x");
                        sumTemp.SetLeftChild(aTemp);
                        sumTemp.SetRightChild(bTemp);
                        sumTemp.SetFreq(aTemp.GetFreq() + bTemp.GetFreq());
                        InsertTree(sumTemp);
                    }
            }
            public HuffmanTree RemoveTree()
            {
                if (!(first == null))
                {
                    HuffmanTree hTemp;
                    hTemp = first.data;
                    first = first.link;
                    count--;
                    return hTemp;
                }
                return null;
            }
            public void InsertTree(HuffmanTree hTemp)
            {
                Node eTemp = new Node(hTemp);
                if (first == null)
                    first = eTemp;
                else
                {
                    Node p = first;
                    while (!(p.link == null))
                    {
                        if ((p.data.GetFreq() <= hTemp.GetFreq()) && (p.link.data.GetFreq() >= hTemp.GetFreq()))
                            break;
                        p = p.link;
                    }
                    eTemp.link = p.link;
                    p.link = eTemp;
                }
                count++;
            }
            public int Length()
            {
                return count;
            }
            /// <summary>
            /// 链表---计数
            /// </summary>
            /// <param name="str"></param>
            public void AddSign(string str)
            {
                if (first == null)
                {
                    AddLetter(str);
                    return;
                }
                Node tmp = first;
                while (tmp != null)
                {
                    if (tmp.data.GetSign() == str)
                    {
                        tmp.data.IncFreq();
                        return;
                    }
                    tmp = tmp.link;
                }
                AddLetter(str);
            }
            public static string Translate(string original)
            {
                string newStr = "";
                for (int i = 0; i <= original.Length - 1; i++)
                {
                    for (int j = 0; j <= signTable.Length - 1; j++)
                    {
                        if (original[i].ToString() == signTable[j])
                        {
                            newStr += keyTable[j];
                        }
                    }
                }
                return newStr;
            }
            private static int pos = 0;
            public static void MakeKey(HuffmanTree tree, string code)
            {
                if (tree.GetLeftChild() == null)
                {
                    signTable[pos] = tree.GetSign();
                    keyTable[pos] = code;
                    pos++;
                }
                else
                {
                    MakeKey(tree.GetLeftChild(), code + "0");
                    MakeKey(tree.GetRightChild(), code + "1");
                }
            }
        }
        public class HuffmanTree
        {
            private HuffmanTree leftChild;
            private HuffmanTree rightChild;
            private string letter;
            private int freq;
            public HuffmanTree(string letter)
            {
                this.letter = letter;
                this.freq = 1;
            }
            public void SetLeftChild(HuffmanTree newChild)
            {
                leftChild = newChild;
            }
            public void SetRightChild(HuffmanTree newChild)
            {
                rightChild = newChild;
            }
            public void SetLetter(string newLetter)
            {
                letter = newLetter;
            }
            public void IncFreq()
            {
                freq++;
            }
            public void SetFreq(int newFreq)
            {
                freq = newFreq;
            }
            public HuffmanTree GetLeftChild()
            {
                return leftChild;
            }
            public HuffmanTree GetRightChild()
            {
                return rightChild;
            }
            public int GetFreq()
            {
                return freq;
            }
            public string GetSign()
            {
                return letter;
            }
        }
        #endregion

        #region 背包
        public class Carpet : IComparable
        {
            private string item;
            private float val;
            private int unit;
            public Carpet(string i, float v, int u)
            {
                item = i;
                val = v;
                unit = u;
            }
            public int CompareTo(Object c)
            {
                return (this.val.CompareTo(((Carpet)c).val));
            }
            public int GetUnit()
            {
                return unit;
            }
            public string GetItem()
            {
                return item;
            }
            public float GetVal()
            {
                return val * unit;
            }
            public float ItemVal()
            {
                return val;
            }
        }
        public class Knapsack
        {
            private float quantity;
            SortedList items = new SortedList();
            string itemList;
            public Knapsack(float max)
            {
                quantity = max;
            }
            public void FillSack(ArrayList objects)
            {
                int pos = objects.Count - 1;
                int totalUnits = 0;
                float totalVal = 0.0F;
                int tempTot = 0;
                while (totalUnits < quantity)
                {
                    tempTot += ((Carpet)objects[pos]).GetUnit();
                    if (tempTot <= quantity)
                    {
                        totalUnits += ((Carpet)objects[pos]).GetUnit();
                        totalVal += ((Carpet)objects[pos]).GetVal();
                        items.Add(((Carpet)objects[pos]).GetItem(), ((Carpet)objects[pos]).GetUnit());
                    }
                    else
                    {
                        float tempUnit = quantity - totalUnits;
                        float tempVal = ((Carpet)objects[pos]).ItemVal() * tempUnit;
                        totalVal += tempVal;
                        totalUnits += (int)tempUnit;
                        items.Add(((Carpet)objects[pos]).GetItem(), tempUnit);
                    }
                    pos--;
                }
            }
            public string GetItems()
            {
                foreach (Object k in items.GetKeyList())
                    itemList += k.ToString() + ": " + items[k].
                    ToString() + " ";
                return itemList;
            }
        }

        #endregion
        #endregion

        #region  进阶排序算法
        public static void Show2()
        {
            int[] array = new int[10];
            for (int i = 0; i < array.Length; i++)
            {
                Thread.Sleep(100);
                array[i] = new Random(i + 100 + DateTime.Now.Millisecond).Next(100, 999);
            }

            Console.WriteLine("before AdvancedSort");
            array.Show();
            Console.WriteLine("start AdvancedSort");
            //array.ShellSort();
            //array.MergeSort();
            //array.HeapSort();
            //array.QuickSort();
            Console.WriteLine("  end AdvancedSort");
            array.Show();
        }

        #region 希尔排序
        /// <summary>
        /// 插入排序
        /// </summary>
        /// <param name="arr"></param>
        private static void InsertionSortOld(this int[] arr)
        {
            int inner, temp;
            for (int outer = 1; outer <= arr.Length - 1; outer++)
            {
                temp = arr[outer];//6
                inner = outer;
                while (inner > 0 && arr[inner - 1] >= temp)
                {
                    arr[inner] = arr[inner - 1];
                    inner -= 1;
                }
                arr[inner] = temp;
                //arr.Show();
            }
        }
        private static void ShellSort(this int[] arr)
        {
            int inner = 0;
            int temp = 0;
            int increment = 0;

            while (increment <= arr.Length / 3)//10--4      20    13
            {
                increment = increment * 3 + 1;
            }
            while (increment > 0)//4--1
            {
                for (int outer = increment; outer <= arr.Length - 1; outer++)
                {
                    temp = arr[outer];
                    inner = outer;
                    while ((inner > increment - 1) && arr[inner - increment] >= temp)
                    {
                        arr[inner] = arr[inner - increment];
                        inner -= increment;
                    }
                    arr[inner] = temp;
                    arr.Show();
                }//increment=1时就是插入排序一样的代码
                increment = (increment - 1) / 3;
                arr.Show();
            }
        }
        #endregion

        #region 归并排序
        /// <summary>
        /// 归并排序
        /// </summary>
        /// <param name="arr"></param>
        public static void MergeSort(this int[] arr)
        {
            int[] temp = new int[arr.Length];//准备空数组
            PartSort(arr, 0, arr.Length - 1, temp);
        }
        /// <summary>
        /// 递归分治
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="temp"></param>
        private static void PartSort(int[] arr, int left, int right, int[] temp)
        {
            if (left < right)
            {
                int middle = (left + right) / 2;
                PartSort(arr, left, middle, temp);//左边归并排序
                PartSort(arr, middle + 1, right, temp);//右边归并排序
                Merge(arr, left, middle, right, temp);//合并操作
            }
        }
        private static void Merge(int[] arr, int left, int mid, int right, int[] temp)
        {
            int i = left;
            int j = mid + 1;
            int t = 0;
            while (i <= mid && j <= right)
            {
                if (arr[i] <= arr[j])
                {
                    //temp[t++] = arr[i++];
                    temp[t] = arr[i];
                    t++;
                    i++;
                }
                else
                {
                    //temp[t++] = arr[j++];
                    temp[t] = arr[j];
                    t++;
                    j++;
                }
            }
            while (i <= mid)
            {
                temp[t++] = arr[i++];//将左边剩余元素填充进temp中
            }
            while (j <= right)
            {
                temp[t++] = arr[j++];//将右序列剩余元素填充进temp中
            }
            t = 0;
            while (left <= right)
            {
                arr[left++] = temp[t++];//将temp中的元素全部拷贝到原数组中
            }
            //arr.Show();
        }
        #endregion

        #region 堆排序
        /// <summary>
        /// 堆排序
        /// </summary>
        /// <param name="arr"></param>
        public static void HeapSort(this int[] arr)
        {
            //1.构建大顶堆
            for (int i = arr.Length / 2 - 1; i >= 0; i--)
            {
                BuildHeap(arr, i, arr.Length);
            }
            Console.WriteLine("堆构建完成");
            //2.交换堆顶元素与末尾元素
            for (int j = arr.Length - 1; j > 0; j--)
            {
                Swap(arr, 0, j);
                BuildHeap(arr, 0, j);
            }
        }

        /// <summary>
        /// 保证局部大小符合
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="i"></param>
        /// <param name="Length"></param>
        private static void BuildHeap(int[] arr, int i, int Length)
        {
            int temp = arr[i];//枝干节点
            for (int k = i * 2 + 1; k < Length; k = k * 2 + 1)
            {
                //arr[k]子节点  arr[k+1]子节点
                if (k + 1 < Length && arr[k] < arr[k + 1])
                {//两个子节点对比  要最大的k<k+1就把k++
                    k++;
                }
                if (arr[k] > temp)
                {//最大如果大于枝干节点
                    arr[i] = arr[k];
                    i = k;//把i换成k 下面再替换，等于交换值
                }
                else
                {
                    break;
                }
                //继续循环就以刚才节点为枝干节点去比较其子节点，持续交换下去
            }
            arr[i] = temp;
            //arr.Show();
        }
        private static void Swap(int[] arr, int a, int b)
        {
            int temp = arr[a];
            arr[a] = arr[b];
            arr[b] = temp;
        }
        #endregion

        #region 快排
        public static void QuickSort(this int[] arr)
        {
            QuickSortRecursion(arr, 0, arr.Length - 1);
        }

        /// <summary>
        /// 递归排序单个数组
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private static void QuickSortRecursion(int[] arr, int left, int right)
        {
            if (left < right)
            {
                SetReference(arr, left, right);//获取参照物
                int referenceIndex = right - 1;
                int i = left;
                int j = right - 1;
                while (true)
                {
                    while (arr[++i] < arr[referenceIndex])
                    {
                    }
                    while (j > left && arr[--j] > arr[referenceIndex])
                    {
                    }
                    if (i < j)
                    {
                        Swap(arr, i, j);
                        //arr.Show();
                    }
                    else
                    {
                        break;
                    }
                }
                if (i < right)
                {
                    Swap(arr, i, right - 1);
                    //arr.Show();
                }
                QuickSortRecursion(arr, left, i - 1);
                QuickSortRecursion(arr, i + 1, right);
            }
        }
        private static void SetReference(int[] arr, int left, int right)
        {
            int mid = (left + right) / 2;
            if (arr[left] > arr[mid])
            {
                Swap(arr, left, mid);
            }
            if (arr[left] > arr[right])
            {
                Swap(arr, left, right);
            }
            if (arr[right] < arr[mid])
            {
                Swap(arr, right, mid);
            }
            //arr.Show();
            Swap(arr, right - 1, mid);
            //arr.Show();
        }

        #endregion
        #endregion

        #region  BasicSearchDemo
        public static void Show4()
        {
            int[] array = new int[10];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new Random(i + DateTime.Now.Millisecond).Next(0, 100);
            }
            array.Show();
            //int iResult = -1;
            //Console.WriteLine("find your int number");
            //while (iResult < 0)
            //{
            //    Console.WriteLine("please input your int number");
            //    string sValue = Console.ReadLine();
            //    if (!int.TryParse(sValue, out int iVaule))
            //    {
            //        Console.WriteLine("please input right number");
            //    }
            //    else
            //    {
            //        //iResult = array.SequentialSearch(iVaule);
            //        bool bResult = array.SequentialSearchWithSelfOrganizing(iVaule);
            //    }
            //}
            array.InsertionSort();
            array.Show();
            Console.WriteLine("please input your int number");
            string sValue = Console.ReadLine();
            int.TryParse(sValue, out int iVaule);
            array.BinarySearch(iVaule);
            //Array.BinarySearch(array, 123);//内置查找
        }

        /// <summary>
        /// 顺序查找
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="iValue"></param>
        /// <returns>所在索引</returns>
        public static int SequentialSearch(this int[] arr, int iValue)
        {
            for (int index = 0; index < arr.Length; index++)
            {
                if (arr[index] == iValue)
                {
                    return index;
                }
            }
            return -1;
        }


        public static int Min(this int[] arr)
        {
            int min = arr[0];
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i] < min)
                {
                    min = arr[i];
                }
            }
            return min;
        }
        public static int Max(this int[] arr)
        {
            int max = arr[0];
            for (int i = 0; i < arr.Length - 1; i++)
            {

                if (arr[i] > max)
                {
                    max = arr[i];
                }
            }
            return max;
        }

        #region 自组织查找
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public static bool SequentialSearchWithSelfOrganizing(this int[] arr, int sValue)
        {
            for (int index = 0; index < arr.Length - 1; index++)
            {
                if (arr[index] == sValue)
                {
                    if (index > 0)
                    {
                        int temp = arr[index - 1];
                        arr[index - 1] = arr[index];
                        arr[index] = temp;
                        arr.Show();
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 28原则优化
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public static int SequentialSearchWithSelfOrganizing28(this int[] arr, int sValue)
        {
            for (int index = 0; index < arr.Length - 1; index++)
            {
                if (arr[index] == sValue)
                {
                    if (index > (arr.Length * 0.2))
                    {
                        if (index > 0)
                        {
                            int temp = arr[index - 1];
                            arr[index - 1] = arr[index];
                            arr[index] = temp;
                            arr.Show();
                        }
                    }
                    return index;
                }
            }
            return -1;
        }
        #endregion

        #region 二叉查找
        /// <summary>
        /// 二叉查找
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int BinarySearch(this int[] arr, int value)
        {
            int right = arr.Length - 1;
            int left = 0;
            int middle;
            while (left <= right)
            {
                middle = (right + left) / 2;
                if (arr[middle] == value)
                {
                    return middle;
                }
                else if (value < arr[middle])
                {
                    right = middle - 1;
                }
                else
                {
                    left = middle + 1;
                }
            }
            return -1;
        }

        /// <summary>
        /// 递归式二叉查找
        /// </summary>
        /// <param name="value"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int BinarySearchRecursion(this int[] arr, int value, int left, int right)
        {
            if (left > right)
            {
                return -1;
            }
            else
            {
                int middle = (int)(right + left) / 2;
                if (value < arr[middle])
                {
                    return arr.BinarySearchRecursion(value, left, middle - 1);
                }
                else if (value == arr[middle])
                {
                    return middle;
                }
                else
                {
                    return arr.BinarySearchRecursion(value, middle + 1, right);
                }
            }
        }
        #endregion
        #endregion

        #region  BasicSortDemo
        public static void Show5()
        {
            int[] array = new int[10];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new Random(i + DateTime.Now.Millisecond).Next(100, 999);
            }

            Console.WriteLine("before BubbleSort");
            array.Show();
            Console.WriteLine("start BubbleSort");
            //array.BubbleSort();
            //array.SelectionSort();
            array.InsertionSort();

            Console.WriteLine("  end BubbleSort");
            array.Show();
        }


        /// <summary>
        /// 冒泡排序
        /// 先挑最大值 摆在最后面
        /// 先挑最小值 摆在最前面？
        /// </summary>
        /// <param name="arr"></param>
        public static void BubbleSort(this int[] arr)
        {
            int temp;
            for (int outer = arr.Length; outer >= 1; outer--)
            {
                for (int inner = 0; inner <= outer - 1; inner++)
                {
                    if (arr[inner] > arr[inner + 1])
                    {
                        temp = arr[inner];
                        arr[inner] = arr[inner + 1];
                        arr[inner + 1] = temp;
                    }
                }
                arr.Show();
            }
        }
        /// <summary>
        /// 选择排序
        /// 依次选择最小的数字放到最左边
        /// </summary>
        /// <param name="arr"></param>
        public static void SelectionSort(this int[] arr)
        {
            int min, temp;
            for (int outer = 0; outer < arr.Length; outer++)
            {
                min = outer;
                for (int inner = outer + 1; inner < arr.Length; inner++)
                {
                    if (arr[inner] < arr[min])
                    {
                        min = inner;
                    }
                }
                temp = arr[outer];
                arr[outer] = arr[min];
                arr[min] = temp;
                arr.Show();
            }
        }

        /// <summary>
        /// 插入排序
        /// 从第2个数开始，跟第一个数对比，放在左边还是右边
        /// 循环下去比较，都放在合适的位置
        /// </summary>
        /// <param name="arr"></param>
        public static void InsertionSort(this int[] arr)
        {
            int inner, temp;
            for (int outer = 1; outer < arr.Length; outer++)
            {
                temp = arr[outer];
                inner = outer;
                while (inner > 0 && arr[inner - 1] >= temp)
                {
                    arr[inner] = arr[inner - 1];
                    inner -= 1;
                }
                arr[inner] = temp;
                arr.Show();
            }
        }

        private static void Show(this int[] arr)
        {
            foreach (var item in arr)
            {
                Console.Write(item.ToString() + " ");
            }
            Console.WriteLine();
        }
        #endregion
    }


}
