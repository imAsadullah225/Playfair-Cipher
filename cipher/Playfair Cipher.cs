using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cipher
{
    public partial class PlayfairCipher : Form
    {
        public PlayfairCipher()
        {
            InitializeComponent();
        }

        List<char> encryptedText = new  List<char>();
        List<char> decryptedText = new List<char>();
        
        char[] alphabates = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        char[,] playfairMatrix;
        string uniqueKey;

        bool isKeyInsertedIntoMatrix()
        {
            int count = 0;
            uniqueKey = "";
            playfairMatrix = new char[5, 5];
            
            for (int i = 0; i < key_TB.Text.Length; i++)
            {
                if (!uniqueKey.Contains(key_TB.Text[i]))
                    uniqueKey += key_TB.Text[i];
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (count != uniqueKey.Length)
                    {
                        playfairMatrix[i, j] = uniqueKey[count];
                        count++;
                    }
                    else
                        playfairMatrix[i, j] = '-';
                }
            }
            return true;        
        }

        bool isPlayfairMatrixReady()
        {
            int c = 0;
            List<char> uniqueAlpha = new List<char>(); 

            if (isKeyInsertedIntoMatrix() == true)
            {
                for (int i = 0; i < alphabates.Count(); i++)
                {
                    if (!uniqueKey.Contains(alphabates[i]))
                        uniqueAlpha.Add(alphabates[i]);
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (playfairMatrix[i, j] == '-' && uniqueAlpha.Count != c)
                        {
                            playfairMatrix[i, j] = uniqueAlpha[c];
                            c++;
                        }
                    }
                }
                return true;
            }
            else
                return false;
        }

        int countX;
        List<char> bigrams = new List<char>();
        bool check = false;
        void bigram(char[] arr)
        {
            bigrams.Clear();
            countX = 0;

            for (int i = 1; i < arr.Count(); i += 2)
            {
                try
                {
                    if (arr[i - 1] == arr[i] && countX == 0)
                    {
                        bigrams.Add(arr[i - 1]);
                        bigrams.Add('x');
                        bigrams.Add(arr[i]);
                        countX++;
                        check = true;
                    }
                    else
                    {
                        bigrams.Add(arr[i - 1]);
                        bigrams.Add(arr[i]);
                    }
                }
                catch (Exception) { };
            }

            if (arr[arr.Count() - 1] != bigrams[bigrams.Count() - 1])
                bigrams.Add(arr[arr.Count() - 1]);
        }

        public void doEncryption(string text)
        {           
            char[] charArray = text.ToArray();
            List<int> row = new List<int>();
            List<int> column = new List<int>();

            if(isPlayfairMatrixReady() == true)
            {
                //Replace j with i
                for (int i = 0; i < charArray.Count(); i++)
                {
                    if (charArray[i] == 'j')
                        charArray[i] = 'i';
                }

                bigram(charArray);
                
                //String length Even

                if (bigrams.Count() % 2 != 0)
                    bigrams.Add('x');


                if (check == true)
                {
                    bigram(bigrams.ToArray());
                    check = false;
                }

                //Encryption Logic
                for (int i = 0; i < bigrams.Count(); i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            if (bigrams[i] == playfairMatrix[j, k])
                            {
                                row.Add(j);
                                column.Add(k);
                            }
                        }
                    }
                }

                for (int i = 1; i < bigrams.Count(); i+=2)
                {
                    try
                    {
                        //Rule if char in same column
                        if (column[i - 1] == column[i])
                        {
                            if (row[i - 1] + 1 > 4)
                            {
                                encryptedText.Add(playfairMatrix[0, column[i - 1]]);
                                encryptedText.Add(playfairMatrix[row[i] + 1, column[i]]);
                            }

                            if (row[i] + 1 > 4)
                            {
                                encryptedText.Add(playfairMatrix[row[i - 1] + 1, column[i - 1]]);
                                encryptedText.Add(playfairMatrix[0, column[i]]);
                            }

                            else
                            {
                                encryptedText.Add(playfairMatrix[row[i - 1] + 1, column[i - 1]]);
                                encryptedText.Add(playfairMatrix[row[i] + 1, column[i]]);
                            }
                        }

                        //Rule if char in same row
                        else if (row[i - 1] == row[i])
                        {
                            if (column[i - 1] + 1 > 4)
                            {
                                encryptedText.Add(playfairMatrix[row[i - 1], 0]);
                                encryptedText.Add(playfairMatrix[row[i], column[i] + 1]);
                            }

                            if (column[i] + 1 > 4)
                            {
                                encryptedText.Add(playfairMatrix[row[i - 1], column[i - 1] + 1]);
                                encryptedText.Add(playfairMatrix[row[i], 0]);
                            }

                            else
                            {
                                encryptedText.Add(playfairMatrix[row[i - 1], column[i - 1] + 1]);
                                encryptedText.Add(playfairMatrix[row[i], column[i] + 1]);
                            }
                        }

                        else
                        {
                            encryptedText.Add(playfairMatrix[row[i - 1], column[i]]);
                            encryptedText.Add(playfairMatrix[row[i], column[i - 1]]);
                        }
                    }
                    catch (Exception) { };
                }
            }
        }

        public void doDecryption(string text)
        {
            char[] charArray = text.ToArray();
            List<int> row = new List<int>();
            List<int> column = new List<int>();

            if (isPlayfairMatrixReady() == true)
            {
                //Decryption Logic
                for (int i = 0; i < charArray.Count(); i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            if (charArray[i] == playfairMatrix[j, k])
                            {
                                row.Add(j);
                                column.Add(k);
                            }
                        }
                    }
                }

                for (int i = 1; i < charArray.Count(); i += 2)
                {
                    try
                    {
                        //Rule if char in same column
                        if (column[i - 1] == column[i])
                        {
                            if (row[i - 1] - 1 < 0)
                            {
                                decryptedText.Add(playfairMatrix[4, column[i - 1]]);
                                decryptedText.Add(playfairMatrix[row[i] - 1, column[i]]);
                            }

                            if (row[i] - 1 < 0)
                            {
                                decryptedText.Add(playfairMatrix[row[i - 1] - 1, column[i - 1]]);
                                decryptedText.Add(playfairMatrix[4, column[i]]);
                            }

                            else
                            {
                                decryptedText.Add(playfairMatrix[row[i - 1] - 1, column[i - 1]]);
                                decryptedText.Add(playfairMatrix[row[i] - 1, column[i]]);
                            }
                        }

                        //Rule if char in same row
                        else if (row[i - 1] == row[i])
                        {
                            if (column[i - 1] - 1 < 0)
                            {
                                decryptedText.Add(playfairMatrix[row[i - 1], 4]);
                                decryptedText.Add(playfairMatrix[row[i], column[i] - 1]);
                            }

                            if (column[i] - 1 < 0)
                            {
                                decryptedText.Add(playfairMatrix[row[i - 1], column[i - 1] - 1]);
                                decryptedText.Add(playfairMatrix[row[i], 4]);
                            }

                            else
                            {
                                decryptedText.Add(playfairMatrix[row[i - 1], column[i - 1] - 1]);
                                decryptedText.Add(playfairMatrix[row[i], column[i] - 1]);
                            }
                        }

                        else
                        {
                            decryptedText.Add(playfairMatrix[row[i - 1], column[i]]);
                            decryptedText.Add(playfairMatrix[row[i], column[i - 1]]);
                        }
                    }
                    catch (Exception) { };
                }
            }      
        }

        private void encrypt_BTN_Click(object sender, EventArgs e)
        {
            if (key_TB.Text != "")
            {
                doEncryption(inputText_TB.Text);
                string encrypt = string.Join("", encryptedText);
                encryptedText_TB.Text = encrypt;
                encryptedText.Clear();
            }
            else
                MessageBox.Show("Key is empty please enter a key.","Error");
        }

        private void decrypt_BTN_Click(object sender, EventArgs e)
        {
            if (key_TB.Text != "")
            {
                doDecryption(encryptedText_TB.Text);
                string decrypt = string.Join("", decryptedText);
                decryptedText_TB.Text = decrypt;
                decryptedText.Clear();
            }
            else
                MessageBox.Show("Key is empty please enter a key.", "Error");
        }

        private void inputText_TB_TextChanged(object sender, EventArgs e)
        {
            encryptedText.Clear();
            decryptedText.Clear();

            encryptedText_TB.Clear();
            decryptedText_TB.Clear();
        }
    }
}