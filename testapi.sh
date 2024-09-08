API_URL="http://localhost:5000/api"  

echo "Test GET /accounts"
curl -X GET "$API_URL/accounts" -H "Content-Type: application/json"
echo ""

echo "Test POST /accounts"
curl -X POST "$API_URL/accounts" -H "Content-Type: application/json" -d '{
  "username": "newuser",
  "password": "newpassword"
}'
echo ""

echo "Test GET /accounts/1"
curl -X GET "$API_URL/accounts/1" -H "Content-Type: application/json"
echo ""

echo "Test PUT /accounts/1"
curl -X PUT "$API_URL/accounts/1" -H "Content-Type: application/json" -d '{
  "username": "updateduser",
  "password": "updatedpassword"
}'
echo ""

echo "Test DELETE /accounts/1"
curl -X DELETE "$API_URL/accounts/1" -H "Content-Type: application/json"
echo ""
