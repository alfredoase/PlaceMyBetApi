using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MatchAPI.Models
{
    public class MatchsRepository
    {
        private MySqlConnection Connect()
        {
            string connString = "Server=localhost;Port=3306;Database=dbmatches;Uid=root;password=4613255ASE.;SslMode=none";
            MySqlConnection con = new MySqlConnection(connString);

            return con;
        }

        internal List<Match> Retrieve()
        {
            MySqlConnection con = Connect();
            MySqlCommand com = con.CreateCommand();
            com.CommandText = "SELECT * FROM dbmatches.`match`";

            try
            {
                con.Open();
                MySqlDataReader l = com.ExecuteReader();

                Match match = null;
                List<Match> matchList = new List<Match>();

                while (l.Read())
                {
                    match = new Match(l.GetInt32(0), l.GetString(1), l.GetString(2), null);

                    matchList.Add(match);
                }

                con.Close();
                return matchList;
            }
            catch (MySqlException e)
            {
                Debug.WriteLine("ERROR: " + e);
                return null;
            }
        }

        internal List<Match> RetrieveById(int idmatch)
        {
            MySqlConnection con = Connect();
            MySqlCommand com = con.CreateCommand();
            com.CommandText = "SELECT * FROM dbmatches.`match` WHERE `idpartido` = @ID";
            com.Parameters.AddWithValue("@ID", idmatch);

            int idPartido;

            try
            {
                con.Open();
                MySqlDataReader l = com.ExecuteReader();

                Match match = null;
                List<Match> matchList = new List<Match>();

                while (l.Read())
                {
                    idPartido = l.GetInt32(0);

                    match = new Match(l.GetInt32(0), l.GetString(1), l.GetString(2), RetrieveByIdMatch(idPartido));

                    matchList.Add(match);

                }

                con.Close();
                return matchList;
            }
            catch (MySqlException e)
            {
                Debug.WriteLine("ERROR: " + e);
                return null;
            }
        }

        internal List<Market> RetrieveByIdMatch(int idmatch)
        {
            MySqlConnection con = Connect();
            MySqlCommand com = con.CreateCommand();
            com.CommandText = "SELECT * FROM dbmatches.`mercado` WHERE `idpartido` = @ID";
            com.Parameters.AddWithValue("@ID", idmatch);

            try
            {
                con.Open();
                MySqlDataReader l = com.ExecuteReader();

                Market market = null;
                List<Market> marketList = new List<Market>();

                while (l.Read())
                {
                    int marketId = l.GetInt32(0);
                    int partidoId = l.GetInt32(1);
                    double cuotaO = l.GetDouble(2);
                    double cuotaU = l.GetDouble(3);
                    double dineroO = l.GetDouble(4);
                    double dineroU = l.GetDouble(5);
                    double golesMercado = l.GetDouble(6);

                    market = new Market(marketId, partidoId, cuotaO, cuotaU, dineroO, dineroU, golesMercado);

                    marketList.Add(market);
                }

                con.Close();
                return marketList;
            }
            catch (MySqlException e)
            {
                Debug.WriteLine("ERROR: " + e);
                return null;
            }
        }

        internal double CalculosOver(double dineroO, double dineroU)
        {
                //Bloque apuesta Over
                double probabilidadO = dineroO / (dineroO + dineroU);
                double cuotaO = 1 / (probabilidadO * 0.95);

                return cuotaO;
        }

        internal double CalculosUnder(double dineroO, double dineroU)
        {
                //Bloque apuesta Under
                double probabilidadU = dineroU / (dineroO + dineroU);
                double cuotaU = 1 / (probabilidadU * 0.95);

                return cuotaU;
        }

        internal void InsertBet(Boolean isOver, Market market)
        {
            MySqlConnection con = Connect();
            MySqlCommand com = con.CreateCommand();

            int idpartido = market.idPartido;
            int idmarket = market.idMercado;
            double dO = market.dineroO;
            double dU = market.dineroU;
            

            if (isOver)
            {
                com.CommandText = "UPDATE `dbmatches`.`mercado` SET `cuotaO` = @CO, `dineroO` = @DO WHERE `idpartido` = @IDP AND `idmercado` = @IDM";
            }else if (!isOver)
            {
                com.CommandText = "UPDATE `dbmatches`.`mercado` SET `cuotaU` = @CU, `dineroU` = @DU WHERE `idpartido` = @IDP AND `idmercado` = @IDM";
            }

            double cOM = CalculosOver(dO, dU);
            double cUM = CalculosUnder(dO, dU);

            
            com.Parameters.AddWithValue("@IDP", idpartido);
            com.Parameters.AddWithValue("@IDM", idmarket);
            com.Parameters.AddWithValue("@CO", cOM);
            com.Parameters.AddWithValue("@CU", cUM);
            com.Parameters.AddWithValue("@DO", dO);
            com.Parameters.AddWithValue("@DU", dU);

            try
            {
                con.Open();

                com.ExecuteNonQuery();

                con.Close();
            }
            catch (MySqlException e)
            {
                Debug.WriteLine("ERROR: " + e);
            }
        }

    }
}