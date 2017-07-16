using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mts_phone_bot
{
    public partial class Form1 : Form
    {
        Dictionary<long, DateTime> dict = new Dictionary<long, DateTime> { };
        BackgroundWorker bw;
        public Form1()
        {
            InitializeComponent();

            this.bw = new BackgroundWorker();
            this.bw.DoWork += this.bw_DoWork;
        }
        async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var key = e.Argument as String; 
            try
            {
                var Bot = new Telegram.Bot.TelegramBotClient(key); 
                await Bot.SetWebhookAsync("");
               
                int offset = 0; 
                while (true)
                {
                    var updates = await Bot.GetUpdatesAsync(offset); 

                    foreach (var update in updates) 
                    {
                        var message = update.Message;
                        if (message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
                        {
                            if (dict.ContainsKey(message.Chat.Id))
                            {
                                if (message.Date > dict[message.Chat.Id].AddSeconds(10))
                                {
                                    String phone = "";

                                    for (int i = 0; i < message.Text.Length; i++)
                                    {

                                        
                                        if (message.Text[i] >= '0' && message.Text[i] <= '9')
                                        {
                                            phone += message.Text[i];
                                        }

                                    }

                                    if (phone.Length == 10)
                                    {
                                        if (phone[0] == '9')
                                        {
                                            await Bot.SendTextMessage(message.Chat.Id, getPhone(phone), replyToMessageId: message.MessageId);

                                        }
                                        else
                                        {
                                            await Bot.SendTextMessage(message.Chat.Id, "Неверный номер", replyToMessageId: message.MessageId);
                                        }
                                    }

                                    else if (phone.Length == 11)
                                    {
                                        if ((phone[0] == '8' || phone[0] == '7') && phone[1] == '9')
                                        {
                                            await Bot.SendTextMessage(message.Chat.Id, getPhone(phone), replyToMessageId: message.MessageId);
                                        }
                                        else
                                        {
                                            await Bot.SendTextMessage(message.Chat.Id, "Неверный номер", replyToMessageId: message.MessageId);
                                        }
                                    }
                                    else
                                    {
                                        await Bot.SendTextMessage(message.Chat.Id, "Неверный номер", replyToMessageId: message.MessageId);
                                    }

                                    dict[message.Chat.Id]=message.Date;
                                }
                                else
                                {
                                    await Bot.SendTextMessage(message.Chat.Id, "Превышено количество обращений к боту", replyToMessageId: message.MessageId);
                                }
                            }
                            else
                            {
                                String phone = "";

                                for (int i = 0; i < message.Text.Length; i++)
                                {

                                    
                                    if (message.Text[i] >= '0' && message.Text[i] <= '9')
                                    {
                                        phone += message.Text[i];
                                    }

                                }

                                if (phone.Length == 10)
                                {
                                    if (phone[0] == '9')
                                    {
                                        await Bot.SendTextMessage(message.Chat.Id, getPhone(phone), replyToMessageId: message.MessageId);

                                    }
                                    else
                                    {
                                        await Bot.SendTextMessage(message.Chat.Id, "Неверный номер", replyToMessageId: message.MessageId);
                                    }
                                }

                                else if (phone.Length == 11)
                                {
                                    if ((phone[0] == '8' || phone[0] == '7') && phone[1] == '9')
                                    {
                                        await Bot.SendTextMessage(message.Chat.Id, getPhone(phone), replyToMessageId: message.MessageId);
                                    }
                                    else
                                    {
                                        await Bot.SendTextMessage(message.Chat.Id, "Неверный номер", replyToMessageId: message.MessageId);
                                    }
                                }
                                else
                                {
                                    await Bot.SendTextMessage(message.Chat.Id, "Неверный номер", replyToMessageId: message.MessageId);
                                }
                                dict.Add(message.Chat.Id, message.Date);
                            }
                        }
                        
                        offset = update.Id + 1;
                    }

                }
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                Console.WriteLine(ex.Message); 
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            var text = textBox1.Text;
            if (this.bw.IsBusy != true) 
            {
                this.bw.RunWorkerAsync(text); 
            }
        }
        private static String getPhone(String phone)
        {
            String newPhone = "+7(9";
            String p = phone.Substring(phone.Length - 9);
            newPhone += p.Substring(0, 2) + ")" + p.Substring(2, 3) + "-" + p.Substring(5, 2) + "-" + p.Substring(7, 2);
            return newPhone;
        }
    }
}
