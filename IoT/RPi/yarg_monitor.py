import psutil
import paho.mqtt.client as mqtt
import time
import datetime
import ssl
import socket
import subprocess
import uuid
import os
import platform
import json

# CA certificate string
CA_CERT = "/home/rosscj/scripts/ca_certificate.pem"

# MQTT Broker details
MQTT_BROKER = ""
MQTT_PORT = 8883  # Using the secure MQTT port
MQTT_TOPIC = "yargRPI/heartbeat"

# MQTT username and password
MQTT_USERNAME = ""
MQTT_PASSWORD = ""

LWT_TOPIC = "yargRPI/ESTOP"
LWT_QOS = 2
LWT_RETAIN = False

# Function to get system uptime
def get_uptime():
    boot_time_timestamp = psutil.boot_time()
    boot_time = datetime.datetime.fromtimestamp(boot_time_timestamp)
    current_time = datetime.datetime.now()
    uptime = current_time - boot_time
    return str(uptime)

def get_hostname():
    try:
        # Get the host name
        host_name = socket.gethostname()

        return host_name
    except Exception as e:
        print(f"Error retrieving hostname: {e}")
        return None

# Function to run a shell command and capture its output
def run_command(command):
    try:
        result = subprocess.run(command, shell=True, stdout=subprocess.PIPE, text=True)
        return result.stdout.strip()
    except Exception as e:
        print(f"Error executing command: {e}")
        return None

def get_mac_address(interface='eth0'):
    try:
        # Get the MAC address of the specified network interface
        mac = uuid.UUID(int=uuid.getnode()).hex[-12:]
        mac_address = ':'.join([mac[e:e+2] for e in range(0, 12, 2)])

        print(f"Retrieved MAC Address: {mac_address}")  # Add this line
        return mac_address
    except Exception as e:
        print(f"Error retrieving MAC address: {e}")
        return None

def get_voltage():
    try:
        # Get the voltage output from the command
        voltage_output = subprocess.check_output("vcgencmd measure_volts core", shell=True, text=True)

        # Extract the numeric value from the output
        numeric_value = float(voltage_output.split('=')[1].rstrip('V\n'))

        return numeric_value
    except Exception as e:
        print(f"Error retrieving voltage: {e}")
        return None

def get_first_boot_time():
    return datetime.datetime.fromtimestamp(psutil.boot_time()).strftime('%Y-%m-%d %H:%M:%S')

def get_temperature():

    try:
        # Get the temperature output from the command
        temperature_output = subprocess.check_output("vcgencmd measure_temp", shell=True, text=True)

        # Extract the numeric value from the output
        numeric_value = float(temperature_output.split('=')[1].rstrip("'C\n"))

        return numeric_value
    except Exception as e:
        print(f"Error retrieving temperature: {e}")
        return None

# Function to publish system information to MQTT
def on_connect(client, userdata, flags, rc):
    if rc == 0:
        print("Connected to MQTT Broker")
    else:
        print("Connection failed")

def is_yarg_installed():
    try:
        # Use subprocess to run a command to check if yarg.service is installed
        result = subprocess.run("systemctl list-units --type=service --all | grep 'yarg.service'", shell=True, stdout=subprocess.PIPE, text=True)
        return result.returncode == 0  # If return code is 0, yarg.service is installed
    except Exception as e:
        print(f"Error checking if yarg.service is installed: {e}")
        return False

# Function to gather YARG service information
def get_yarg_info():
    try:
        yarg_app_cpu_count_result = run_command("systemctl status yarg.service | grep 'CPU:'")
        yarg_app_cpu_count = yarg_app_cpu_count_result.split(':')[1].strip() if yarg_app_cpu_count_result else None

        yarg_app_status_result = run_command("systemctl status yarg.service | grep 'Active:'")
        yarg_app_status_parts = yarg_app_status_result.split(': ', 1)
        yarg_app_status = yarg_app_status_parts[1].strip() if len(yarg_app_status_parts) > 1 else None

        process_stats = run_command("systemctl status yarg.service | grep 'Tasks:'")
        if process_stats:
            parts = process_stats.split('(')
            if len(parts) == 2:
                yarg_app_current_tasks_str = parts[0].strip().split(':')[1].strip()
                yarg_app_current_tasks = int(yarg_app_current_tasks_str)
                yarg_app_task_limit_str = parts[1].split(':')[1].strip(' )')
                yarg_app_task_limit = int(yarg_app_task_limit_str)
            else:
                yarg_app_current_tasks = None
                yarg_app_task_limit = None
        else:
            yarg_app_current_tasks = None
            yarg_app_task_limit = None

        return yarg_app_cpu_count, yarg_app_status, yarg_app_current_tasks, yarg_app_task_limit
    except Exception as e:
        print(f"Error getting YARG service info: {e}")
        return None, None, None, None

def gather_heartbeat_info():
    heartbeat_info = {
        "MACAddress": get_mac_address(),
        "Hostname": get_hostname(),
        "CPUUsage": psutil.cpu_percent(),
        "MemoryUsage": psutil.virtual_memory().percent,
        "DiskUsage": psutil.disk_usage('/').percent,
        "Temperature": get_temperature(),
        "LoadAverage": os.getloadavg()[2],
        "Voltage": get_voltage()
    }

    # Get YARG service info if available
    if is_yarg_installed():
        yarg_app_cpu_count, yarg_app_status, yarg_app_current_tasks, yarg_app_task_limit = get_yarg_info()
        heartbeat_info["YARGAppCPUCount"] = yarg_app_cpu_count
        heartbeat_info["YARGAppStatus"] = yarg_app_status
        heartbeat_info["YARGAppCurrentTasks"] = yarg_app_current_tasks
        heartbeat_info["YARGAppTaskLimit"] = yarg_app_task_limit

    return heartbeat_info


# Function to gather system information, OS name, and OS version at first boot
def gather_initial_info():
    system_info = {
        "MACAddress": get_mac_address(),
        "Hostname": get_hostname(),
        "OSName": os.name,
        "OSVersion": os.uname().release,
        "CPUModel": run_command("cat /proc/cpuinfo | grep 'model name' | uniq | awk -F ': ' '{print $2}'"),
        "CPUCores": psutil.cpu_count(logical=False),
        "CPUTemperature": get_temperature(),
        "CPUSerialNumber": run_command("cat /proc/cpuinfo | grep 'Serial' | awk -F ': ' '{print $2}'"),
        "TotalRAM": psutil.virtual_memory().total,
        "TotalDiskSpace": psutil.disk_usage('/').total,
        "TotalUsedDiskSpace": psutil.disk_usage('/').used,
        "FirstBootDateTime": get_first_boot_time()
    }

    return system_info

def gather_lwt_payload():
    lwt_info = {
        "Hostname": get_hostname()
    }

    return lwt_info

if __name__ == "__main__":
    # Create a MQTT client instance
    client = mqtt.Client()
    client.on_connect = on_connect

    # Disable TLSv1 and TLSv1.1 and set minimum TLS version to TLSv1.2
    ssl_context = ssl.SSLContext(ssl.PROTOCOL_TLS)
    ssl_context.options |= ssl.OP_NO_TLSv1 | ssl.OP_NO_TLSv1_1
    ssl_context.minimum_version = ssl.TLSVersion.TLSv1_2
    ssl_context.load_verify_locations(cafile=CA_CERT)

    client.tls_set_context(ssl_context)

    # Set MQTT username and password
    client.username_pw_set(MQTT_USERNAME, MQTT_PASSWORD)

    client.will_set(LWT_TOPIC, payload=gather_lwt_payload(), qos=LWT_QOS, retain=LWT_RETAIN)

    # Connect to the broker (localhost)
    client.connect(MQTT_BROKER, MQTT_PORT, keepalive=1)

    # Start the MQTT loop in the background
    client.loop_start()

    # Gather and publish initial system information only once
    initial_info = gather_initial_info()
    initial_info_message = json.dumps(initial_info)
    client.publish("yargRPI/hello", initial_info_message)

    # Wait for a few seconds to allow the first message to be published
    time.sleep(5)

    # Continue publishing system heartbeat every 15 minutes
    while True:
        heartbeat_info = gather_heartbeat_info()
        heartbeat_info_message = json.dumps(heartbeat_info)
        client.publish("yargRPI/heartbeat", heartbeat_info_message)
        time.sleep(900)  # 15 minutes = 900 seconds