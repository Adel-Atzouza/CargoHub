import requests
import pytest

BASE_URL = "http://localhost:3000/api/v1/orders"
ITEMS_URL = "http://localhost:3000/api/v1/items"
HEADERS = {'APIKEY': "Admin"}

# Helper function: Cleanup orders and items
def cleanup_test_data():
    # Cleanup orders
    orders_response = requests.get(BASE_URL, headers=HEADERS)
    if orders_response.status_code == 200:
        for order in orders_response.json():
            if "Test" in order.get("reference", ""):
                requests.delete(f"{BASE_URL}/{order['id']}", headers=HEADERS)

    # Cleanup items
    items_response = requests.get(ITEMS_URL, headers=HEADERS)
    if items_response.status_code == 200:
        for item in items_response.json():
            if "TEST_ITEM" in item.get("uid", ""):
                requests.delete(f"{ITEMS_URL}/{item['uid']}", headers=HEADERS)

# Helper function: Create test items
def create_test_items():
    test_items = [
        {"uid": "TEST_ITEM_001", "description": "Test Item 1", "unitOrderQuantity": 10},
        {"uid": "TEST_ITEM_002", "description": "Test Item 2", "unitOrderQuantity": 5}
    ]
    for item in test_items:
        response = requests.post(ITEMS_URL, json=item, headers=HEADERS)
        if response.status_code not in [200, 201]:
            print(f"Failed to create test item {item['uid']}: {response.text}")

# Test: Add a new order
def test_add_order():
    cleanup_test_data()
    create_test_items()

    new_order = {
        "sourceId": 999,
        "orderDate": "2024-12-01T00:00:00Z",
        "requestDate": "2024-12-10T00:00:00Z",
        "reference": "Test Order",
        "orderStatus": "TEST",  # Verplicht veld toegevoegd
        "notes": "Test notes",  # Verplicht veld toegevoegd
        "pickingNotes": "Picking note example",  # Verplicht veld toegevoegd
        "shippingNotes": "Shipping note example",  # Verplicht veld toegevoegd
        "items": [
            {"itemId": "TEST_ITEM_001", "amount": 2},
            {"itemId": "TEST_ITEM_002", "amount": 1}
        ]
    }

    response = requests.post(BASE_URL, json=new_order, headers=HEADERS)
    print(response.json())  # Debug response
    print("Status code:", response.status_code)
    print("Response text:", response.text)  # Debug server response
    assert response.status_code == 201, f"Add order failed: {response.text}"

    # Cleanup test data
    cleanup_test_data()


# Test: Retrieve all orders
def test_get_all_orders():
    cleanup_test_data()
    create_test_items()

    # Add a test order
    test_add_order()

    response = requests.get(BASE_URL, headers=HEADERS)
    print(response.json())  # Debug response
    assert response.status_code == 200
    orders = response.json()
    assert len(orders) > 0

    cleanup_test_data()

# Test: Delete an order
def test_delete_order():
    cleanup_test_data()
    create_test_items()

    # Add a test order
    new_order = {
        "sourceId": 999,
        "orderDate": "2024-12-01T00:00:00Z",
        "requestDate": "2024-12-10T00:00:00Z",
        "reference": "Test Delete Order",
        "items": [{"itemId": "TEST_ITEM_001", "amount": 1}]
    }
    response = requests.post(BASE_URL, json=new_order, headers=HEADERS)
    assert response.status_code == 201
    order_id = response.json()["id"]

    # Delete the order
    delete_response = requests.delete(f"{BASE_URL}/{order_id}", headers=HEADERS)
    assert delete_response.status_code in [200, 204]

    # Verify deletion
    fetch_response = requests.get(f"{BASE_URL}/{order_id}", headers=HEADERS)
    assert fetch_response.status_code == 404

    cleanup_test_data()