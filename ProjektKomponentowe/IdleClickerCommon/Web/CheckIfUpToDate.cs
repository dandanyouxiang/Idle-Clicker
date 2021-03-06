﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleClickerCommon
{
    public class CheckIfUpToDate : IUpdaterAction
    {
        public event OnEndUpdateActionDelegate OnEndUpdateAction;
        public event OnProgressDelegate OnProgress1;
        public event OnProgressDelegate OnProgress2;
        public event OnStartUpdateActionDelegate OnStartUpdateAction;

        public UpdaterActionSetting GetSettings()
        {
            return new UpdaterActionSetting()
            {
                Progress1IsIndeterminate = false,
                Progress1Visible = false,
                Progress2IsIndeterminate = true,
                Progress2Visible = true
            };
        }

        public void Start(string destination)
        {
            try
            {
                if (OnStartUpdateAction != null)
                    OnStartUpdateAction();
                UpdateModule.CheckIfUpToDate();
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (OnEndUpdateAction != null)
                    OnEndUpdateAction();
            }
        }
    }
}
