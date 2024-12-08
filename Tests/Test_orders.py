import requests
import pytest

BASE_URL = "http://localhost:3000/api/v1/orders" 
headers = {'APIKEY': "Admin"}


# Helper function to clean up test data by deleting any orders with "Test" in their reference
def cleanup_test_data(test_name):
    response = requests.get(BASE_URL, headers=headers)  # Send GET request to fetch all orders
    if response.status_code == 200:
        orders = response.json()  # Parse the response as JSON to get the list of orders
        for order in orders:
            # Check if the order is test data based on its reference (contains "Test")
            if "Test" in order["reference"]:
                delete_response = requests.delete(f"{BASE_URL}/{order['id']}", headers=headers)  # Send DELETE request to remove the order by ID
                assert delete_response.status_code in [200, 204], f"Cleanup failed in {test_name}"  # Assert successful deletion


# Test to retrieve all orders
def test_get_all_orders():
    response = requests.get(BASE_URL, headers=headers)  # Send GET request to fetch all orders
    assert response.status_code == 200, f"Failed to fetch orders: {response.text}"  # Assert that the response status is 200 (OK)
    orders = response.json()  # Parse the response as JSON to get the list of orders
    assert isinstance(orders, list)  # Assert that the response is a list of orders
    assert len(orders) > 0  # Assert that the list of orders is not empty


# Test to retrieve a specific order by ID
def test_get_order_by_id():
    order_id = 1  # Set a valid order ID for testing
    response = requests.get(f"{BASE_URL}/{order_id}", headers=headers)  # Send GET request to fetch the order by ID
    assert response.status_code == 200, f"Failed to fetch order: {response.text}"  # Assert that the response status is 200 (OK)
    order = response.json()  # Parse the response as JSON
    assert order["id"] == order_id  # Assert that the order's ID matches the requested ID


# Test to add a new order
def test_add_order():
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
    response = requests.post(BASE_URL, json=new_order, headers=headers)  # Send POST request to create a new order
    assert response.status_code == 201, f"Failed to add order: {response.text}"  # Assert that the response status is 201 (Created)

    # Print the response JSON for debugging purposes
    print(response.json())

    # Verify that the response contains the added order
    order = response.json()
    assert order["reference"] == new_order["reference"]  # Assert that the reference matches the new order's reference

    # Check if 'items' key exists in the response
    if "items" in order:
        assert len(order["items"]) == len(new_order["items"])  # Assert that the number of items in the response matches the input
    else:
        print("Warning: 'items' key not found in the response")  # If no items are found, print a warning message

    cleanup_test_data("test_add_order")  # Clean up the test data after the test


# Test to get an order by an invalid ID
def test_get_order_by_invalid_id():
    invalid_id = 9999
    response = requests.get(f"{BASE_URL}/{invalid_id}", headers=headers)  # Send GET request to fetch the order by invalid ID
    assert response.status_code == 404  # Assert that the response status is 404 (Not Found)


# Test to remove an order by an invalid ID
def test_remove_order_invalid_id():
    invalid_id = 9999
    response = requests.delete(f"{BASE_URL}/{invalid_id}", headers=headers)  # Send DELETE request to remove the order by invalid ID
    assert response.status_code == 404  # Assert that the response status is 404 (Not Found)
