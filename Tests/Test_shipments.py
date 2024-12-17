import requests
import pytest

BASE_URL = "http://localhost:3000/api/v1/Shipments"
headers = {'APIKEY': "Admin"}

# Helper function to clean up test data by deleting shipments with "Test" in their shipmentType
def cleanup_test_data(test_name):
    response = requests.get(BASE_URL, headers=headers)  # Send GET request to fetch all shipments
    if response.status_code == 200:
        shipments = response.json()  # Parse the response as JSON to get the list of shipments
        for shipment in shipments:
            # Check if the shipment is test data based on its shipmentType (contains "Test")
            if "Test" in shipment["shipmentType"]:
                delete_response = requests.delete(f"{BASE_URL}/{shipment['id']}", headers=headers)  # Send DELETE request to remove the shipment by ID
                assert delete_response.status_code in [200, 204], f"Cleanup failed in {test_name}"  # Assert successful deletion


# Test to retrieve all shipments
def test_get_all_shipments():
    response = requests.get(BASE_URL, headers=headers)  # Send GET request to fetch all shipments
    assert response.status_code == 200  # Assert that the response status is 200 (OK)
    shipments = response.json()  # Parse the response as JSON
    assert isinstance(shipments, list)  # Assert that the response is a list of shipments
    assert len(shipments) > 0  # Assert that the list of shipments is not empty


# Test to retrieve a specific shipment by ID
def test_get_shipment_by_id():
    shipment_id = 1
    response = requests.get(f"{BASE_URL}/{shipment_id}", headers=headers)  # Send GET request to fetch the shipment by ID
    assert response.status_code == 200  # Assert that the response status is 200 (OK)
    shipment = response.json()  # Parse the response as JSON
    assert shipment["id"] == shipment_id  # Assert that the shipment's ID matches the requested ID


# Test to add a new shipment
def test_add_shipment():
    new_shipment = {
        "sourceId": 999,  
        "orderDate": "2024-12-01T00:00:00Z",
        "requestDate": "2024-12-10T00:00:00Z", 
        "shipmentDate": "2024-12-15T00:00:00Z",
        "shipmentType": "Test Shipment",
        "shipmentStatus": "TEST", 
        "notes": "Test notes",
        "carrierCode": "TEST_CARRIER",  
        "carrierDescription": "Test Carrier Description",  
        "serviceCode": "TEST_SERVICE",  
        "paymentType": "Prepaid",  
        "transferMode": "Air", 
        "totalPackageCount": 5,  
        "totalPackageWeight": 50.0 
    }
    response = requests.post(BASE_URL, json=new_shipment, headers=headers)  # Send POST request to create a new shipment
    assert response.status_code == 201, f"Failed to add shipment: {response.text}"  # Assert that the response status is 201 (Created)

    # Print the response JSON for debugging purposes
    print(response.json())

    # Verify that the response contains the added shipment
    shipment = response.json()
    assert shipment["shipmentType"] == new_shipment["shipmentType"]  # Assert that the shipment type matches the new shipment's shipmentType

    cleanup_test_data("test_add_shipment")  # Clean up the test data after the test