import requests

url = "http://13.210.249.205:8080/api/contacts/47e56289-6760-4646-ba8d-0c35ffdb8062"

payload = {}
headers = {
  'Accept': 'text/plain',
  'Cookie': 'Created-By=Vinod; Powered-By=Springboot'
}

response = requests.request("GET", url, headers=headers, data=payload)

print(response.text)
