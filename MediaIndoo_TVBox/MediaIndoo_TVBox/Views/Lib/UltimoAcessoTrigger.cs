using MediaIndoo_TVBox.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MediaIndoo_TVBox.Views.Lib
{
    public class UltimoAcessoTrigger : TriggerAction<Entry>
    {
        protected async override void Invoke(Entry sender)
        {
            //Task.Delay(5000);
            if (sender.Text != null)
            {

                var result = await new PlayeReqService().GetAllPlayeReqsId(int.Parse(sender.Text));
                if (result == null)
                {
                    sender.BackgroundColor = Color.White;
                }
                else
                {

                    if (result.IsSuccess)
                    {
                        var min = result.Data.QtdMinutos;
                        if (min <= 10)
                        {
                            sender.BackgroundColor = Color.Red;
                            sender.IsEnabled = true;
                        }
                        else
                            sender.BackgroundColor = Color.Green;
                    }
                }
            }

        }
    }
}
