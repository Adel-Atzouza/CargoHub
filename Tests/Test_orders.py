import requests
import pytest

BASE_URL = "http://localhost:3000/api/v1/orders"  # Pas aan op basis van je API-host en poort
headers = {'APIKEY': "Admin"}


def cleanup_test_data(test_name):
    "Cleanup test orders with reference containing 'Test'."
    response = requests.get(BASE_URL, headers=headers)
    if response.status_code == 200:
        orders = response.json()
        for order in orders:
            # Check if the order is a test record
            if "Test" in order["reference"]:
                delete_response = requests.delete(f"{BASE_URL}/{order['id']}", headers=headers)
                assert delete_response.status_code in [200, 204], f"Cleanup failed in {test_name}"


def test_get_all_orders():
    "Test retrieving all orders."
    response = requests.get(BASE_URL, headers=headers)
    assert response.status_code == 200, f"Failed to fetch orders: {response.text}"
    orders = response.json()
    assert isinstance(orders, list)
    assert len(orders) > 0


def test_get_order_by_id():
    "Test retrieving an order by ID."
    order_id = 1 
    response = requests.get(f"{BASE_URL}/{order_id}", headers=headers)
    assert response.status_code == 200, f"Failed to fetch order: {response.text}"
    order = response.json()
    assert order["id"] == order_id

    

def test_add_order():
    "Test adding a new order."
    new_order = {
        "sourceId": 999,
        "orderDate": "2024-12-01T00:00:00Z",
        "requestDate": "2024-12-10T00:00:00Z",
        "reference": "Test Order",
        "orderStatus": "TEST",
        "totalAmount": 100.0,
        "totalDiscount": 0.0,
        "totalTax": 0.0,
        "totalSurcharge": 0.0,
        "notes": "Test notes",
        "pickingNotes": "Test picking notes",
        "shippingNotes": "Test shipping notes",
        "items": [
            {"itemId": "TEST_ITEM_001", "amount": 2},
            {"itemId": "TEST_ITEM_002", "amount": 1}
        ]
    }
    response = requests.post(BASE_URL, json=new_order, headers=headers)
    assert response.status_code == 201, f"Failed to add order: {response.text}"

    # Print the response JSON for debugging
    print(response.json())

    # Verify the response contains the added order
    order = response.json()
    assert order["reference"] == new_order["reference"]

    # Check if 'items' key exists in the response
    if "items" in order:
        assert len(order["items"]) == len(new_order["items"])
    else:
        print("Warning: 'items' key not found in the response")

    cleanup_test_data("test_add_order")


# def test_update_order():
#     response = requests.get(BASE_URL, headers=headers)
#     assert response.status_code == 200, f"Failed to fetch orders: {response.text}"

#     orders = response.json()
#     order_id = None

#     # Find the test order to update
#     for order in orders:
#         if "Test" in order["reference"]:
#             order_id = order["id"]
#             break

#     assert order_id is not None, "No test order found to update."

#     updated_order = {
#         "reference": "Test Order Updated",
#         "orderStatus": "Updated",
#         "totalAmount": 150.0,
#         "items": [
#             {"itemId": "TEST_ITEM_001", "amount": 3},
#             {"itemId": "TEST_ITEM_002", "amount": 2}
#         ]
#     }
#     response = requests.put(f"{BASE_URL}/{order_id}", json=updated_order, headers=headers)
#     assert response.status_code == 200, f"Failed to update order: {response.text}"

#     # Verify the response contains the updated order
#     order = response.json()
#     assert order["reference"] == updated_order["reference"]
#     assert order["totalAmount"] == updated_order["totalAmount"]


def test_get_order_by_invalid_id():
    "Test retrieving an order with an invalid ID."
    invalid_id = 9999 
    response = requests.get(f"{BASE_URL}/{invalid_id}", headers=headers)
    assert response.status_code == 404


def test_remove_order_invalid_id():
    "Test removing an order with an invalid ID."
    invalid_id = 9999
    response = requests.delete(f"{BASE_URL}/{invalid_id}", headers=headers)
    assert response.status_code == 404


