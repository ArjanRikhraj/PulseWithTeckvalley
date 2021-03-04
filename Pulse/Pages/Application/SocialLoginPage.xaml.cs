using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Pulse
{
    public partial class SocialLoginPage : BaseContentPage
    {
        public string ProviderName { get; set; }
        public SocialLoginPage()
        {
            InitializeComponent();
        }
        public SocialLoginPage(string _providername) : this()
        {
            ProviderName = _providername;
        }
    }
}
