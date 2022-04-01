﻿using System;
using System.Data.SqlTypes;
using System.IO;
using System.Net;
using Microsoft.SqlServer.Server;

public class StoreSKU
{
    private const String PUBLIC_API = "https://publicapi.abaha.vn/";
    private const String PATH = "/Product/create";
    private const String COOKIE = "tickid_session=ra675ab7g7puklmbdanpbko76fvhcjh4";

    // tooken
    private const String accessToken =
        "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczpcL1wvYWJhaGEudm5cLyIsImF1ZCI6Imh0dHBzOlwvXC9hYmFoYS52blwvIiwiZXhwIjoxNjc3MDc0MzA3LCJqYXQiOjE2NDU1MzgzMDcsImFwcF9rZXkiOiJlYXJsZG9tLXBodWtpZW5kaWVudGhvYWlrZXlEbkE4biJ9.KCjwc7hoi2Sdh96hFCJiv01Q5SuRDvfTk64Y43ngntU";

    [SqlProcedure]
    // email is email of staff
    // code is code of router on bravo
    public static void CreateSku(String name, String unit, String code, String price, String madein, String brand
            , out SqlString text)
        // )
    {
        // ssl2
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = (SecurityProtocolType) (0xc0 | 0x300 | 0xc00);
        // initialization  request
        var httpWebRequest = (HttpWebRequest) WebRequest.Create(PUBLIC_API + PATH);
        httpWebRequest.ContentType = "application/json";
        // add authorization to header
        httpWebRequest.Headers["Authorization"] = accessToken;
        httpWebRequest.Headers["Cookie"] = COOKIE;
        // add method
        httpWebRequest.Method = "POST";
        // use stream write
        string status;
        try
        {
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = GetBodySku(name.Trim(), unit.Trim(), code.Trim(), price.Trim(), madein.Trim(),
                    brand.Trim());
                streamWriter.Write(json);
                streamWriter.Close();
            }

            var response = (HttpWebResponse) httpWebRequest.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (StreamReader streamReader =
                       new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    status = streamReader.ReadToEnd();
                    streamReader.Close();
                }
            }
            else
            {
                status = "501";
            }

            response.Close();
        }
        catch (Exception e)
        {
            Console.Out.WriteLine(e.ToString());
            status = "502";
        }

        // return status;
        text = status;
    }


    private static string GetBodySku(String name, String unit, String code, String price, String madein,
        String brand)
    {
        return "{" +
               "\"name\":\"" + name + "\"," +
               "\"ordering\": 20," +
               "\"unit_name\":\"" + unit + "\"," +
               "\"product_code\":\"" + code + "\"," +
               "\"price\":" + price + "," +
               "\"price\":" + price + "," +
               "\"made_in\":\"" + madein + "\"," +
               "\"brand\":\"" + brand + "\"," +
               "\"category_id\": 0" +
               "}";
    }
}