using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json.Linq;
using static UserDataConnection;

public class UserDataConnection : MonoBehaviour
{
    public static UserDataConnection instance;

    [SerializeField] private TMP_Text idtext;
    [SerializeField] private TMP_InputField pwtext;

    [SerializeField] private GameObject loginAnnouncement;
    [SerializeField] private Text loginMessage;
    [SerializeField] private Text loginMessageWhy;

    public bool isLogin { get; set; } = false;

    public int scorePoint;

    public string userID;
    public MyLoginResponseData myLoginResponseData;
    public updateParams myUpdateParams;

    // create, login
    public class loginParams 
    {
        public string id { get; set; }
        public string pw { get; set; }

    }

    public class loginRequest 
    {
        public string api { get; set; }
        public loginParams @params { get; set; }
    }

    public class MyLoginResponse
    {
        public string api { get; set; }
        public int code { get; set; }

        public MyLoginResponseData data;

    }

    public class MyLoginResponseData
    {
        public string token { get; set; }
        public int score { get; set; }
        public string msg { get; set; }
       
    }

    public class logoutParams
    {
        public string id { get; set; }
        public string token { get; set; }

    }

    // logout
    public class logoutRequest
    {
        public string api { get; set; }
        public logoutParams @params { get; set; }
    }

    public class MyLogoutResponse
    {
        public string api { get; set; }
        public int code { get; set; }
        public MyLogoutData data { get; set; }
    }
    public class MyLogoutData
    {
        public string msg { get; set; }
    }

    // update
    public class updateParams
    {
        public string id { get; set; }
        public string token { get; set; }
        public int point { get; set; }
    }

   
    public class updateRequest
    {
        public string api { get; set; }
        public updateParams @params { get; set; }
    }


    public class MyUpdateResponse
    {
        public string api { get; set; }
        public int code { get; set; }
        public MyUpdateResponseData data;
    }

    public class MyUpdateResponseData
    {
        public string msg { get; set; }
        public int score { get; set; }

    }


    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    

    public async void CreateAccountOnClicked()
    {
        string str_id = idtext.text;
        string str_pw = pwtext.text;

        Debug.Log(str_id);
        Debug.Log(str_pw);

        await api_create_account(str_id, str_pw); // 비동기 완료
    }

    public async void LoginOnClicked()
    {
        string str_id = idtext.text;
        string str_pw = pwtext.text;

        Debug.Log(str_id);
        Debug.Log(str_pw);

        myLoginResponseData = new MyLoginResponseData();

        await api_login(str_id, str_pw); // 비동기 완료

       // Debug.Log(myparams.score);
       // Debug.Log(myparams.token);
    }

    public async void QuitGameOnClicked()
    {
        string str_id = userID;
        string str_token = myLoginResponseData.token;
        int n_point = GameManager.instance.killCount;

        Debug.Log(str_id);
        Debug.Log(str_token);

        await api_logout(str_id, str_token);
    }

    public async void UserUpdateScore()
    {
        string str_id = userID;
        string str_token = myLoginResponseData.token;
        int n_point = GameManager.instance.killCount;

        Debug.Log(str_id);
        Debug.Log(str_token);
        Debug.Log(n_point);

        await api_update_score(str_id, str_token, n_point);
    }

    public async Task api_create_account(string id, string pw) // 비동기 대기 (백그라운드에서 시행되는 방식)
    {
        // POST할 URL
        string url = "http://ted-rpi4-dev.duckdns.org:62431/api/create_account";

        try
        {
            
            // 보낼 json 데이터 생성
            loginRequest postdata = new loginRequest()
            {
                api = "create_account",
                @params = new loginParams()
                {
                    id = id,
                    pw = pw
                },
            };

            // json 데이터를 문자열로 변환
            string json = JsonConvert.SerializeObject(postdata);

            // JSON 데이터를 StringContent로 변환
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // HttpClient 인스턴스 생성
            using (HttpClient client = new HttpClient())
            {
                // POST 요청 보내기
                HttpResponseMessage response = await client.PostAsync(url, content);

                // 응답 확인
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.Log("서버 응답:");
                    Debug.Log(responseBody);

                    loginMessage.text = "계정 생성이 완료되었습니다.";
                    loginMessageWhy.text = "";
                }
                else
                {
                    Debug.Log("서버 오류: " + response.StatusCode);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.Log("서버 응답:");
                    Debug.Log(responseBody);

                    loginMessage.text = "계정 생성이 실패하였습니다.";
                }
                loginAnnouncement.SetActive(true);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("에러 발생: " + ex.Message);
        }
    }


    public async Task api_login(string id, string pw) 
    {
        // POST할 URL
        string url = "http://ted-rpi4-dev.duckdns.org:62431/api/login";

        try
        {

            // 보낼 json 데이터 생성
            loginRequest postdata = new loginRequest()
            {
                api = "login",
                @params = new loginParams()
                {
                    id = id,
                    pw = pw
                },
            };

            // json 데이터를 문자열로 변환
            string json = JsonConvert.SerializeObject(postdata);

            // JSON 데이터를 StringContent로 변환
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // HttpClient 인스턴스 생성
            using (HttpClient client = new HttpClient())
            {
                // POST 요청 보내기
                HttpResponseMessage response = await client.PostAsync(url, content);

                // 응답 확인
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.Log("서버 응답:");
                    Debug.Log(responseBody);

                    // JSON을 객체로 역직렬화
                    MyLoginResponse responseData = JsonConvert.DeserializeObject<MyLoginResponse>(responseBody);

                    myLoginResponseData.token = responseData.data.token;
                    myLoginResponseData.score = responseData.data.score;
                    // 추가로 받아온 token과 score 출력
                    userID = id;

                    Debug.Log("Given Score: " + responseData.data.score.ToString());
                    Debug.Log("Current Score: " + myLoginResponseData.score.ToString());


                    Debug.Log("Given Token: " + responseData.data.token);
                    Debug.Log("Current Token: " + myLoginResponseData.token);

                    Debug.Log(userID);

                    loginMessage.text = "로그인에 성공하였습니다.";
                    isLogin = true;
                    loginAnnouncement.SetActive(true);

                }
                else
                {
                    Debug.Log("서버 오류: " + response.StatusCode);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.Log("서버 응답:");
                    Debug.Log(responseBody);
                    loginMessage.text = "로그인에 실패하였습니다.";
                }
                loginAnnouncement.SetActive(true);

            }
        }
        catch (Exception ex)
        {
            Debug.Log("에러 발생: " + ex.Message);
        }
    }

    public async Task api_logout(string id, string token)
    {
        // POST할 URL
        string url = "http://ted-rpi4-dev.duckdns.org:62431/api/logout";

        try
        {
            // 보낼 json 데이터 생성
            logoutRequest postdata = new logoutRequest()
            {
                api = "logout",
                @params = new logoutParams()
                {
                    id = id,
                    token = token,
                },
            };

            // json 데이터를 문자열로 변환
            string json = JsonConvert.SerializeObject(postdata);

            // JSON 데이터를 StringContent로 변환
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // HttpClient 인스턴스 생성
            using (HttpClient client = new HttpClient())
            {
                // POST 요청 보내기
                HttpResponseMessage response = await client.PostAsync(url, content);

                // 응답 확인
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.Log("서버 응답:");
                    Debug.Log(responseBody);

                    // JSON을 객체로 역직렬화
                    MyLogoutResponse responseData = JsonConvert.DeserializeObject<MyLogoutResponse>(responseBody);

                    // 추가로 받아온 token과 score 출력
                    MyLogoutResponse myLogout = new MyLogoutResponse();
                    //myLogout.data.msg = responseData.data.msg;
                    Debug.Log($" 확인용 msg: {responseData.data.msg}");

                    //Console.WriteLine("Message : " + responseData.data.msg);


                }
                else
                {
                    Debug.Log("서버 오류: " + response.StatusCode);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.Log("서버 응답:");
                    Debug.Log(responseBody);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("에러 발생: " + ex.Message);
        }
    }

    public async Task api_update_score(string id, string token, int point)
    {
        // POST할 URL
        string url = "http://ted-rpi4-dev.duckdns.org:62431/api/update_score";

        try
        {
            // 보낼 json 데이터 생성
            updateRequest postdata = new updateRequest()
            {
                api = "update_score",
                @params = new updateParams()
                {
                    id = id,
                    token = token,
                    point = point
                },
            };

            // json 데이터를 문자열로 변환
            string json = JsonConvert.SerializeObject(postdata);

            // JSON 데이터를 StringContent로 변환
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // HttpClient 인스턴스 생성
            using (HttpClient client = new HttpClient())
            {
                // POST 요청 보내기
                HttpResponseMessage response = await client.PostAsync(url, content);

                // 응답 확인
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.Log("서버 응답:");
                    Debug.Log(responseBody);

                    // JSON을 객체로 역직렬화
                    MyUpdateResponse responseData = JsonConvert.DeserializeObject<MyUpdateResponse>(responseBody);

                    // 추가로 받아온 token과 score 출력
                    myLoginResponseData.score = responseData.data.score;
                    Debug.Log(" 확인용 Score: " + responseData.data.score.ToString());

                    //Console.WriteLine("Message : " + responseData.data.msg);

                }
                else
                {
                    Debug.Log("서버 오류: " + response.StatusCode);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.Log("서버 응답:");
                    Debug.Log(responseBody);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("에러 발생: " + ex.Message);
        }
    }


}
