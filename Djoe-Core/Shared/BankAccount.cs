﻿using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Models
{
    public class BankAccountHistory
    {
        #region Variables
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public string Details { get; set; }
        #endregion
    }

    public enum AccountType
    {
        Personal,
        Society,
        Faction,
        Business
    }

    public class BankAccount
    {
        #region Delegates
        public delegate Task OnDepositDelagate(Player client);
        public delegate Task OnWithdrawDelegate(Player client);
        #endregion

        #region Static fields
        public static List<BankAccount> BankAccountsList = new List<BankAccount>();
        #endregion

        #region Fields
        /*
        private DateTime _lastUpdateRequest;
        private bool _updateWaiting = false;
        private int _nbUpdateRequests;*/
        #endregion

        #region Properties
        public AccountType AccountType { get; set; }
        public string AccountNumber { get; set; }
        public double Balance { get; private set; }
        public object Owner { get; set; }

        [JsonIgnore]
        public OnDepositDelagate OnDeposit { get; set; }
        public OnWithdrawDelegate OnWithdraw { get; set; }

        public List<BankAccountHistory> History { get; private set; }
        #endregion

        #region Constructor
        public BankAccount(AccountType type, string accountNumber, double balance = 0)
        {
            AccountType = type;
            AccountNumber = accountNumber;
            Balance = balance;
            History = new List<BankAccountHistory>();
        }
        #endregion

        #region Method
        public void AddMoney(double money, string reason, bool save = true)
        {
            Balance += money;
            History.Add(new BankAccountHistory()
            {
                Amount = money,
                Balance = this.Balance,
                Date = DateTime.Now,
                Text = reason
            });
            /*
            if (save)
                UpdateInBackground();*/
        }

        public void AddMoney(double money, bool save = true)
        {
            Balance += money;
            /*
            if (save)
                UpdateInBackground();*/
        }

        public bool GetBankMoney(double money, string reason, string details = null, bool save = true)
        {
            if (money == 0)
                return true;

            if (Balance - money >= 0)
            {
                Balance -= money;
                History.Add(new BankAccountHistory()
                {
                    Amount = money,
                    Balance = this.Balance,
                    Date = DateTime.Now,
                    Text = reason,
                    Details = details
                });

                /*
                if (save)
                    UpdateInBackground();*/

                return true;
            }

            return false;
        }

        public static string GenerateNewAccountNumber()
        {
            string _accountNumber = GenerateString();

            while (BankAccountsList.Exists(b => b.AccountNumber == _accountNumber))
            {
                _accountNumber = GenerateString();
            }

            return _accountNumber;
        }

        public void Clear() => Balance = 0;

        private static string GenerateString() =>
            $"{new Random().Next(1000000, 9999999)}";


        /* A voir si necessaire ?
        public void UpdateInBackground()
        {
            _lastUpdateRequest = DateTime.Now;

            if (_updateWaiting)
            {
                _nbUpdateRequests++;
                return;
            }

            _updateWaiting = true;
            _nbUpdateRequests = 1;

            Task.Run(async () =>
            {
                DateTime updateTime = _lastUpdateRequest.AddMilliseconds(Globals.SAVE_WAIT_TIME);

                while (DateTime.Now < updateTime)
                {
                    TimeSpan waitTime = updateTime - DateTime.Now;

                    if (waitTime.TotalMilliseconds < 1)
                        waitTime = new TimeSpan(0, 0, 0, 0, 1);

                    await Task.Delay((int)waitTime.TotalMilliseconds);
                    updateTime = _lastUpdateRequest.AddMilliseconds(Globals.SAVE_WAIT_TIME);
                }

                try
                {
                    var result = await Database.MongoDB.UpdateBankAccount(this, _nbUpdateRequests);

                    if (result.MatchedCount == 0)
                        Logger.Warn($"Update error for bank account {AccountNumber}");

                    _updateWaiting = false;
                }
                catch (Exception ex)
                {
                    Logger.Warn($"BankAccount.UpdateInBackground() - {AccountNumber} - {ex}");
                }
            });
        }*/
        #endregion
    }
}
